using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileProcessor
{
    [XmlRoot("DEPLOYMENTDATA")]
    public class DeploymentData
    {
        [XmlElement("VEHICLEDATA")]
        public List<VehicleData> VehicleData { get; set; }

        [XmlElement("DEPLOYMENTSUMMARY")]
        public DeploymentSummary DeploymentSummary { get; set; }
    }

    public class VehicleData
    {
        [XmlElement("SEQUENCE")]
        public long Sequence { get; set; }

        [XmlElement("VEHICLESPEED")]
        public int VehicleSpeed { get; set; }

        [XmlElement("TIMESTAMP")]
        public string Timestamp { get; set; }

        [XmlElement("IMAGE")]
        public string Image { get; set; }

        [XmlElement("VIDEO")]
        public string Video { get; set; }

        [XmlElement("JMX")]
        public string Jmx { get; set; }

        [XmlElement("TXT")]
        public string Txt { get; set; }

        [XmlElement("CROSSHAIRX")]
        public int CrosshairX { get; set; }

        [XmlElement("CROSSHAIRY")]
        public int CrosshairY { get; set; }
    }

    public class DeploymentSummary
    {
        [XmlElement("CAMERANAME")]
        public string CameraName { get; set; }

        [XmlElement("LOCATIONCODE")]
        public string LocationCode { get; set; }

        [XmlElement("LOCATION")]
        public string Location { get; set; }

        [XmlElement("DEPLOYMENTID")]
        public string DeploymentId { get; set; }

        [XmlElement("STARTSEQUENCE")]
        public long StartSequence { get; set; }

        [XmlElement("ENDSEQUENCE")]
        public long EndSequence { get; set; }

        [XmlElement("SPEEDLIMIT")]
        public int SpeedLimit { get; set; }

        [XmlElement("CAPTURESPEED")]
        public int CaptureSpeed { get; set; }

        [XmlElement("MEASUREMENTUNIT")]
        public string MeasurementUnit { get; set; }

        [XmlElement("OPERATORID")]
        public string OperatorId { get; set; }

        [XmlElement("OPERATORNAME")]
        public string OperatorName { get; set; }

        [XmlElement("STARTDTM")]
        public string StartDtm { get; set; }

        [XmlElement("ENDDTM")]
        public string EndDtm { get; set; }
    }
}
