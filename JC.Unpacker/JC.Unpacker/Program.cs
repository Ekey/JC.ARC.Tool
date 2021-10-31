using System;
using System.IO;

namespace JC.Unpacker
{
    class Program
    {
        static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Just Cause TAB/ARC Unpacker");
            Console.WriteLine("(c) 2021 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    JC.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of TAB archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    JC.Unpacker E:\\Games\\JC\\Archives\\pc.tab D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_TabFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_TabFile))
            {
                Utils.iSetError("[ERROR]: Input TAB file -> " + m_TabFile + " <- does not exist");
                return;
            }

            TabUnpack.iDoIt(m_TabFile, m_Output);
        }
    }
}
