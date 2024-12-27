using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FileProcessor.SDK
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct textdataW
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string ClipType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string ClipNumber;
        public int iNumberOfFrames;
        public int iMeasurementFrame;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string SpeedLimit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string CaptureSpeed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string SpeedUnits;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string DistanceUnits;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredSpeed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredDistance;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredSpeed2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredDistance2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredTBC;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredDBC;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredTBM;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string MeasuredRoadOffset;
        public int iCurrentLane;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string OperatorName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
        public string OperatorID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string StreetName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string StreetCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string ClipDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string ClipTime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string LastAligned;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string CalExpires;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public string PaidData;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string Latitude;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string Longitude;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string FirmwareVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string SerialNo;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
        public string Signature;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string SystemMode;
        public int iCrosshairX;
        public int iCrosshairY;
        public int iImageWidth;
        public int iImageHeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string LowerSpeedLimit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string HigherSpeedLimit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string LowerCaptureSpeed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string HigherCaptureSpeed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string LimitsUsed;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
        public string ClipName;
        public int iVehicleType;
    }
}
