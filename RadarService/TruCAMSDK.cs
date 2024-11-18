using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarService
{
    public static class TruCAMSDK
    {
        public static TextDataStructure GetTextData()
        {
            var textData = new TextDataStructure();
            int result = SDKInterop.GetTextDataW(ref textData);

            if (result != 0)
                throw new Exception("Failed to retrieve text data.");

            return textData;
        }

        public static Bitmap GetClipFrame(int crosshairType)
        {
            Bitmap frame = new Bitmap(1280, 960);
            int result = SDKInterop.GetClipFrameX(crosshairType, ref frame);

            if (result != 0)
                throw new Exception("Failed to retrieve clip frame.");

            return frame;
        }

        public static Bitmap GetMeasurementFrameWithCrosshair()
        {
            Bitmap frame = new Bitmap(1280, 960);
            int result = SDKInterop.GetMeasurementFrameWithCrosshairX(ref frame);

            if (result != 0)
                throw new Exception("Failed to retrieve measurement frame with crosshair.");

            return frame;
        }
    }
}
