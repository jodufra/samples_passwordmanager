using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GDPLibrary.Utils
{
    public class Security
    {
        public static String GetMD5Hash(String input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            Byte[] originalBytes = Encoding.Default.GetBytes(input);
            Byte[] encodedBytes = sha256.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
    }
}
