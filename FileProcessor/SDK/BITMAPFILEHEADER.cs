using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.SDK
{
    public struct BITMAPINFOHEADER
    {
        [MarshalAs(UnmanagedType.I4)]
        public int biSize;
        [MarshalAs(UnmanagedType.I4)]
        public int biWidth;
        [MarshalAs(UnmanagedType.I4)]
        public int biHeight;
        [MarshalAs(UnmanagedType.I2)]
        public short biPlanes;
        [MarshalAs(UnmanagedType.I2)]
        public short biBitCount;
        [MarshalAs(UnmanagedType.I4)]
        public int biCompression;
        [MarshalAs(UnmanagedType.I4)]
        public int biSizeImage;
        [MarshalAs(UnmanagedType.I4)]
        public int biXPelsPerMeter;
        [MarshalAs(UnmanagedType.I4)]
        public int biYPelsPerMeter;
        [MarshalAs(UnmanagedType.I4)]
        public int biClrUsed;
        [MarshalAs(UnmanagedType.I4)]
        public int biClrImportant;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public short bfType;
        public int bfSize;
        public short bfReserved1;
        public short bfReserved2;
        public int bfOffBits;
    }
}
