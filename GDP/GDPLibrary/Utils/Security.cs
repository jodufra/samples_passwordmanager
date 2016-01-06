using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GDPLibrary.Utils
{
    public class Security
    {
        private static byte[] sharedSalt = Encoding.ASCII.GetBytes("v3rys01tys01t");

        public static String GetSHA256SaltyPassword(String username, String password, out String salt)
        {
            salt = CreateSalt(username);
            return GetSHA256SaltyHashFromTastlessHash(password, salt);
        }

        public static String GetSHA256SaltyHashFromTastlessHash(String input, String salt)
        {
            return GetSHA256Hash(input + salt + sharedSalt);
        }

        public static String GetSHA256Hash(String input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            Byte[] originalBytes = Encoding.Default.GetBytes(input);
            Byte[] encodedBytes = sha256.ComputeHash(originalBytes);
            string result = "";
            foreach (byte x in encodedBytes)
            {
                result += String.Format("{0:x2}", x);
            }
            return result;
        }

        private static string CreateSalt(string s)
        {
            string str = s;
            byte[] userBytes;
            string salt;
            userBytes = ASCIIEncoding.ASCII.GetBytes(str);
            long XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            Random rand = new Random(Convert.ToInt32(XORED));
            salt = rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            salt += rand.Next().ToString();
            return salt;
        }

    }
}
