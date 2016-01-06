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

        public static byte[] EncryptAES(byte[] source, string publicKey)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey");

            byte[] result = null; RijndaelManaged aesAlg = null;
            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(publicKey, sharedSalt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(source);
                        }
                    }
                    result = msEncrypt.ToArray();
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return result;
        }

        public static byte[] DecryptAES(byte[] source, string publicKey)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey");

            RijndaelManaged aesAlg = null;

            byte[] result = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(publicKey, sharedSalt);

                using (MemoryStream msDecrypt = new MemoryStream(source))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    string str = null;
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            str = srDecrypt.ReadToEnd();
                    }

                    result = new byte[str.Length * sizeof(char)];
                    System.Buffer.BlockCopy(str.ToCharArray(), 0, result, 0, result.Length);
                }
            }
            finally
            {
                if (aesAlg != null)
                    aesAlg.Clear();
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

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }

        private static byte[] StringToByteArray(String s)
        {
            byte[] result = new byte[s.Length * sizeof(char)];
            System.Buffer.BlockCopy(s.ToCharArray(), 0, result, 0, result.Length);
            return result;
        }
    }
}
