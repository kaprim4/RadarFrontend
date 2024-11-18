using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class PagableDTO<T> where T : class
    {
        public int Page { get; set; } = 1;
        public int Length { get; set; } = 10;
        public int TotalContent { get; set; }
        public string OrderBy { get; set; } 
        public bool IsAscending { get; set; } = true;
        public string SearchTerm { get; set; } = null;

        public List<T> Content { get; set; }
    }
}
