using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RadarService
{
    //[DllImport("ltijmx64.dll")]
    public static class SDKInterop
    {
        [DllImport("ltijmx64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTextDataW(ref TextDataStructure data);

        [DllImport("ltijmx64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetTextDataW2(ref TextDataStructure data);

        [DllImport("ltijmx64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetClipFrameX(int crosshairType, ref Bitmap frame);

        [DllImport("ltijmx64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMeasurementFrameWithCrosshairX(ref Bitmap frame);

        [DllImport("ltijmx64.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMeasurementFrameWithCrosshairAndTextX(ref Bitmap frame, ref TextDataStructure data);




    }
}
