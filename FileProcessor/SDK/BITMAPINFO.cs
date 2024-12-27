using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.SDK
{
    public struct BITMAPINFO
    {
        [MarshalAs(UnmanagedType.Struct)]
        public BITMAPINFOHEADER bmiHeader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
        public int[] bmiColors;
    }
}
