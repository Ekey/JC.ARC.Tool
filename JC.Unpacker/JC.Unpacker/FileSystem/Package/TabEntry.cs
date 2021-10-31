using System;

namespace JC.Unpacker
{
    class TabEntry
    {
        public UInt32 dwHash { get; set; }
        public UInt32 dwRawOffset { get; set; }
        public UInt32 dwOffset { get; set; }
        public Int32 dwSize { get; set; }
        public Int32 dwArchiveID { get; set; }
    }
}
