using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string? CreatedDate { get; set; }
        public string? BgColor { get; set; }
        public bool IsActive { get; set; }

        public List<Lot>? Lots { get; set; }
    }
}
