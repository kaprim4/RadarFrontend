using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using FileProcessor.SDK;
using Domain.DTO;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

namespace FileProcessor
{
    public class JmxProcessor
    {
        public static string OutPutDirectory = @"D:\me\ali\Narsa\Files\OutPut"; // Define your output directory
        public static int TotalFile = 0;
        public static int Ok = 0;
        public static int Percentage = 0;
        public static List<DeploymentData> DoWork(string[] args)
        {
            List<DeploymentData> output = new();
            TotalFile = args.Length;
            foreach (var item in args)
            {
                string fileName = string.Copy(item);  // Get file path from args
                textdataW userdata = new textdataW();

                if (Lti.GetTextDataW(fileName, ref userdata) == 0)
                {

                    // Extract frames and populate VehicleData list
                    var vehicleDataList = new List<VehicleData>();
                    var imagesList = new List<IFormFile>();
                    byte[] imgbuf = new byte[3686400];  // Buffer for image data
                    BITMAPINFO bmiImage = new BITMAPINFO();
                    string frameTimestamp = new string(' ', 12);  // Timestamp for the frame
                    int frameCounter = 0;

                    while (frameCounter < userdata.iNumberOfFrames)
                    {
                        if (Lti.GetClipFrameWithRangeAndTextNet(fileName, frameCounter, 0, ref bmiImage, imgbuf, frameTimestamp, new string(' ', 12)) == 0)
                        {
                            long sequance = GenerateUniqueSequence(DateTime.Now, frameCounter);
                            string imageName = $"{sequance}_{DateTime.Now:ddMMyyyyHHmmssfff}.bmp";
                            // Save image for each frame
                            string imagePath = Path.Combine(Environment.CurrentDirectory, "Images", Path.GetFileName(item), imageName);
                            Task.Run(()=>SaveBmpImage(imagePath, bmiImage, imgbuf));


                            // Populate VehicleData for each frame
                            var vehicleData = new VehicleData
                            {
                                Sequence = sequance,
                                VehicleSpeed = int.Parse(userdata.MeasuredSpeed),
                                Timestamp = frameTimestamp,
                                Image = imageName,
                                ImageFile = ConvertBytesToIFormFile(imgbuf, imagePath, "image/bmp", imageName),
                                Video = "",
                                Jmx = Path.GetFileName(fileName),
                                Txt = "",
                                CrosshairX = userdata.iCrosshairX,
                                CrosshairY = userdata.iCrosshairY
                            };
                            vehicleDataList.Add(vehicleData);
                            frameCounter++;
                        }
                        else
                        {
                            Console.WriteLine("Failed to extract frame " + frameCounter);
                            break;
                        }
                    }

                    // Create the XML file using the DTOs
                    output.Add(CreateXml(userdata, vehicleDataList, imagesList));
                }
                else
                {
                    Console.WriteLine($"Failed to process file: {fileName}");
                }

                Ok++;
                Percentage = (Ok * 100) / TotalFile;
                Console.WriteLine(Percentage + "%");
            }

            return output;
        }

        private async static Task SaveBmpImage(string filePath, BITMAPINFO bmiImage, byte[] imgbuf)
        {
            BITMAPFILEHEADER bmpHeader = new BITMAPFILEHEADER
            {
                bfType = 19778, // BM
                bfSize = Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + bmiImage.bmiHeader.biSize +
                         bmiImage.bmiHeader.biClrUsed * 4 + bmiImage.bmiHeader.biSizeImage,
                bfReserved1 = 0,
                bfReserved2 = 0,
                bfOffBits = Marshal.SizeOf(typeof(BITMAPFILEHEADER)) + bmiImage.bmiHeader.biSize +
                            bmiImage.bmiHeader.biClrUsed * 4
            };

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[3686400];
                var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                try
                {
                    Marshal.StructureToPtr(bmpHeader, handle.AddrOfPinnedObject(), false);
                    fs.Write(buffer, 0, Marshal.SizeOf(bmpHeader));
                    Marshal.StructureToPtr(bmiImage, handle.AddrOfPinnedObject(), true);
                    fs.Write(buffer, 0, Marshal.SizeOf(bmiImage.bmiHeader) + bmiImage.bmiHeader.biClrUsed * 4);
                    fs.Write(imgbuf, 0, bmiImage.bmiHeader.biSizeImage);
                }
                finally
                {
                    handle.Free();
                }
            }
        }

        // Create the XML using the DTOs
        private static DeploymentData CreateXml( textdataW userdata, List<VehicleData> vehicleDataList, List<IFormFile> Images)
        {
            // Populate DeploymentSummary
            var deploymentSummary = new DeploymentSummary
            {
                CameraName = userdata.SerialNo,
                LocationCode = userdata.StreetCode,
                Location = userdata.StreetName,
                DeploymentId = userdata.ClipName,
                StartSequence = vehicleDataList.FirstOrDefault()?.Sequence ?? 0,
                EndSequence = vehicleDataList.LastOrDefault()?.Sequence ?? 0,
                SpeedLimit = int.Parse(userdata.SpeedLimit),
                CaptureSpeed = int.Parse(userdata.CaptureSpeed),
                MeasurementUnit = userdata.SpeedUnits,
                OperatorId = userdata.OperatorID,
                OperatorName = userdata.OperatorName,
                StartDtm = userdata.ClipDate + " " + userdata.ClipTime,
                EndDtm = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") // Example end time
            };

            // Create DeploymentData object
            return  new DeploymentData
            {
                VehicleDatas = vehicleDataList,
                DeploymentSummary = deploymentSummary,
                Images = Images
            };

            // Serialize to XML
            //string xmlFilePath = Path.Combine(outputFolder, "Output.xml");
            //using (var fileStream = new FileStream(xmlFilePath, FileMode.Create))
            //{
            //    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DeploymentData));
            //    serializer.Serialize(fileStream, deploymentData);
            //}
        }

        private static long GenerateUniqueSequence(DateTime baseTime, int frameIndex)
        {
            // Convert the timestamp to Unix time in seconds
            long unixTime = ((DateTimeOffset)baseTime).ToUnixTimeSeconds();

            // Combine with frame index to ensure uniqueness
            // Adjust the multiplier (e.g., 100, 1000) based on expected frame counts per second
            return unixTime * 100 + frameIndex;
        }


        public static string GetDevice(string filePath)
        {
            textdataW userdata = new textdataW();

            if (Lti.GetTextDataW(filePath, ref userdata) == 0)
            {
                return userdata.SerialNo;
            }

            return null;
        }

        public static IFormFile ConvertBytesToIFormFile(byte[] imageBytes, string fileName, string contentType, string imageName)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image byte array is null or empty.", nameof(imageBytes));
            }

            // Create a memory stream from the byte array
            var memoryStream = new MemoryStream(imageBytes);

            // Create an IFormFile from the memory stream
            var formFile = new FormFile(memoryStream, 0, memoryStream.Length, imageName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }
    }
}
