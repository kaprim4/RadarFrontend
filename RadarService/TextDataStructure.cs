using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RadarService
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct TextDataStructure
    {
        public int Id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Info;
        // Define other fields based on the SDK header file
    }
}
