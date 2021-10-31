using System;

namespace JC.Unpacker
{
    class TabHeader
    {
        public Int32 dwVersion { get; set; } // 3
        public UInt32 dwAligment { get; set; } // 2048
        public UInt32 dwTotalArchives { get; set; }
        public Int32 dwTotalFiles { get; set; }
    }
}
