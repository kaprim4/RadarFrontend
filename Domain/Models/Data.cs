using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string CreatedDate { get; set; }
        public int NbFiles { get; set; }
    }
}
