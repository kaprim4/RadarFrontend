using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Radar.Utilities
{
    public static class SecureStringUtil
    {
        public static SecureString ConvertToSecureString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return new SecureString();

            var secureString = new SecureString();
            foreach (char c in plainText)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }

        public static string ConvertToString(SecureString secureString)
        {
            // This method should only be used when absolutely necessary
            // as it temporarily exposes the secure string in memory
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
