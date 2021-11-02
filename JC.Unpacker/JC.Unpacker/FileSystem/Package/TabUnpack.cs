using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace JC.Unpacker
{
    class TabUnpack
    {
        static List<TabEntry> m_EntryTable = new List<TabEntry>();
        static List<TabArchive> m_Archives = new List<TabArchive>();

        public static void iDoIt(String m_TabFile, String m_DstFolder)
        {
            TabHashList.iLoadProject();
            using (FileStream TTabStream = File.OpenRead(m_TabFile))
            {
                var lpHeader = TTabStream.ReadBytes(12);
                var m_Header = new TabHeader();

                using (var THeaderReader = new MemoryStream(lpHeader))
                {
                    m_Header.dwVersion = THeaderReader.ReadInt32();
                    m_Header.dwAligment = THeaderReader.ReadUInt32();
                    m_Header.dwTotalArchives = THeaderReader.ReadUInt32();

                    if (m_Header.dwVersion != 3)
                    {
                        Utils.iSetError("[ERROR]: Invalid version of TAB index file -> " + m_Header.dwVersion.ToString() + ", expected 3");
                        return;
                    }

                    if (m_Header.dwAligment != 2048)
                    {
                        Utils.iSetError("[ERROR]: Invalid aligment of TAB index file -> " + m_Header.dwAligment.ToString() + ", expected 2048");
                        return;
                    }

                    THeaderReader.Dispose();
                }

                m_Archives.Clear();
                UInt32 dwAligment = 0;
                for (Int32 i = 0; i < m_Header.dwTotalArchives; i++)
                {
                    String m_ArchiveFile = Path.GetDirectoryName(m_TabFile) + @"\" + Path.GetFileNameWithoutExtension(m_TabFile) + i.ToString() + ".arc";
                    using (FileStream TArcStream = File.OpenRead(m_ArchiveFile))
                    {
                        dwAligment += ((UInt32)TArcStream.Length + m_Header.dwAligment - 1) / m_Header.dwAligment;

                        var TArchive = new TabArchive
                        {
                            m_Archive = Path.GetFileName(m_ArchiveFile),
                            dwSize = TArcStream.Length,
                            dwAligment = dwAligment,
                        };

                        m_Archives.Add(TArchive);
                        TArcStream.Dispose();
                    }
                }

                m_Header.dwTotalFiles = ((Int32)TTabStream.Length - (Int32)TTabStream.Position) / 12;

                m_EntryTable.Clear();
                var lpTable = TTabStream.ReadBytes(m_Header.dwTotalFiles * 12);
                using (var TEntryReader = new MemoryStream(lpTable))
                {
                    for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                    {
                        UInt32 dwHash = TEntryReader.ReadUInt32();
                        UInt32 dwRawOffset = TEntryReader.ReadUInt32();
                        Int32 dwSize = TEntryReader.ReadInt32();
                        Int32 dwArchiveID = 0;

                        foreach (var m_Archive in m_Archives)
                        {
                            if (dwRawOffset < m_Archive.dwAligment)
                            {
                                dwArchiveID = m_Archives.ToList().IndexOf(m_Archive);
                                break;
                            }
                        }

                        UInt32 dwOffset = 0;
                        if (dwArchiveID == 0) { dwOffset = (dwRawOffset * m_Header.dwAligment) % 0x40000000; }
                        else { dwOffset = (dwRawOffset - m_Archives[(Int32)dwArchiveID - 1].dwAligment) * m_Header.dwAligment % 0x40000000; }

                        var TEntry = new TabEntry
                        {
                            dwHash = dwHash,
                            dwRawOffset = dwRawOffset,
                            dwOffset = dwOffset,
                            dwSize = dwSize,
                            dwArchiveID = dwArchiveID,
                        };

                        m_EntryTable.Add(TEntry);
                    }

                    TEntryReader.Dispose();
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = TabHashList.iGetNameFromHashList(m_Entry.dwHash);
                    String m_FullPath = m_DstFolder + m_FileName.Replace("/", @"\");

                    //Utils.iSetInfo("[DEBUG]: " + m_FileName + " , RawOffset: " + m_Entry.dwRawOffset.ToString("X8") + " , Offset: " + m_Entry.dwOffset.ToString("X8") + " , Size: " + m_Entry.dwSize + " , Archive ID: " + m_Entry.dwArchiveID.ToString());

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    if (!File.Exists(m_FullPath))
                    {
                        Utils.iCreateDirectory(m_FullPath);

                        using (FileStream TArcStream = File.OpenRead(Path.GetDirectoryName(m_TabFile) + @"\" + "pc" + m_Entry.dwArchiveID.ToString() + ".arc"))
                        {
                            TArcStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                            var lpBuffer = TArcStream.ReadBytes(m_Entry.dwSize);
                            File.WriteAllBytes(m_FullPath, lpBuffer);

                            TArcStream.Dispose();
                        }
                    }
                }
            }
        }
    }
}
    