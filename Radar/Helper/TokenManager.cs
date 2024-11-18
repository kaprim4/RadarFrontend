using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Helper
{
    public static class TokenManager
    {
        public static string JwtToken
        {
            get => Properties.Settings.Default.JwtToken;
            set
            {
                Properties.Settings.Default.JwtToken = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
