using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.DTO
{

    public class TreatmentDTO
    {
        public int Id { get; set; }
        public List<DeploymentData> Deployments { get; set; } = new List<DeploymentData>();
    }

    [XmlRoot("DEPLOYMENTDATA")]
    public class DeploymentData
    {
        [XmlElement("VEHICLEDATA")]
        public List<VehicleData> VehicleDatas { get; set; }
        public List<IFormFile> Images { get; set; }

        [XmlElement("DEPLOYMENTSUMMARY")]
        public DeploymentSummary DeploymentSummary { get; set; }
    }

    //public class VehicleData
    //{
    //    public int Id { get; set; }
    //    [XmlElement("SEQUENCE")]
    //    public long Sequence { get; set; }

    //    [XmlElement("VEHICLESPEED")]
    //    public int VehicleSpeed { get; set; }

    //    [XmlElement("TIMESTAMP")]
    //    public string Timestamp { get; set; }

    //    [XmlElement("IMAGE")]
    //    public string Image { get; set; }

    //    [XmlElement("VIDEO")]
    //    public string Video { get; set; }

    //    [XmlElement("JMX")]
    //    public string Jmx { get; set; }

    //    [XmlElement("TXT")]
    //    public string Txt { get; set; }

    //    [XmlElement("CROSSHAIRX")]
    //    public int CrosshairX { get; set; }

    //    [XmlElement("CROSSHAIRY")]
    //    public int CrosshairY { get; set; }
    //}

    public class VehicleData
    {
        public int Id { get; set; }
        public long Sequence { get; set; }
        public int VehicleSpeed { get; set; }
        public string Timestamp { get; set; }
        public string Image { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Video { get; set; }
        public string Jmx { get; set; }
        public string Txt { get; set; }
        public int CrosshairX { get; set; }
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
