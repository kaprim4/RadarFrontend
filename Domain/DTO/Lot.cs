using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class Lot
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string CreateDate { get; set; }
        public string UpdatedAt { get; set; }
        public int NbFiles
        {
            get
            {
                return Documents.Count;
            }
            set
            {
                NbFiles = value;
            }
        }
        public virtual List<Document> Documents { get; set; }
        public Device? Device { get; set; }
    }

    public class Document
    { 
        public int Id { get; set; }
        public int? LotId { get; set; }
        public string? Reference { get; set; }
        public string Name { get; set; }
        public string Path { get; set; } = "";
        public virtual JMX Jmx { get; set; }

    }

    public class JMX 
    {
        public int Id { get; set; }
        public string? Reference { get; set; }
        public virtual List<VehicleData>? VehicleDatas { get; set; }
        public virtual DeploymentSummary? DeploymentSummary { get; set; }

    }
}
