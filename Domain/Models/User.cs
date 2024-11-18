using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }


        public string Character { get; set; }
        public Brush BgColor { get; set; }
        public int Order { get; set; } = 0;


    }
}
