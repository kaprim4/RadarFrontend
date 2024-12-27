using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.SDK
{
    public class Lti
    {
        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetTextDataW(string FileName, ref textdataW userdata);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetTextDataW2(string FileName, ref textdataW userdata, int dataformat);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetMeasurementFrameWithCrosshairNet(
          string FileName,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetMeasurementFrameWithCrosshairAndTextNet(
          string FileName,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetLastFrameWithCrosshairNet(
          string FileName,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetLastFrameWithCrosshairAndTextNet(
          string FileName,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClipFrameNet(
          string FileName,
          int FrameCounter,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClipFrameWithTextNet(
          string FileName,
          int FrameCounter,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClipFrameWithRangeNet(
          string FileName,
          int FrameCounter,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp,
          string FrameRange);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClipFrameWithRangeAndTextNet(
          string FileName,
          int FrameCounter,
          int CrosshairType,
          ref BITMAPINFO bmiImage,
          byte[] imgbuf,
          string FrameTimeStamp,
          string FrameRange);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int ConvertNamesToTrucamFormatNet(
          string PCFileName,
          string TrucamFileName,
          int Encryption);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int ConvertNamesFromTrucamFormatNet(
          string TrucamFileName,
          string PCFileName,
          int Encryption);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int ConvertLocationsToTrucamFormatNet(
          string PCFileName,
          string TrucamFileName);

        [DllImport("ltijmx64.dll", CharSet = CharSet.Unicode)]
        public static extern int ConvertLocationsFromTrucamFormatNet(
          string TrucamFileName,
          string PCFileName);
    }




    
}
