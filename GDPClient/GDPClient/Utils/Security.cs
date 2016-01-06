using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

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
            HashAlgorithmProvider sha256 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            IBuffer originalBytes = CryptographicBuffer.DecodeFromBase64String(input);
            IBuffer encodedBytes = sha256.HashData(originalBytes);
            return CryptographicBuffer.EncodeToHexString(encodedBytes);
        }

        private static IBuffer GetMD5Hash(string key)
        {
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }
            return buffHash;
        }

        public static string EncryptAES(string source, string publicKey)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey");
            try
            {
                var keyHash = GetMD5Hash(publicKey);
                var toDecryptBuffer = CryptographicBuffer.ConvertStringToBinary(source, BinaryStringEncoding.Utf8);
                var aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);
                // The input key must be securely shared between the sender of the cryptic message
                // and the recipient. The initialization vector must also be shared but does not
                // need to be shared in a secure manner. If the sender encodes a message string
                // to a buffer, the binary encoding method must also be shared with the recipient.
                var buffEncrypted = CryptographicEngine.Encrypt(symetricKey, toDecryptBuffer, null);
                // Convert the encrypted buffer to a string (for display).
                // We are using Base64 to convert bytes to string since you might get unmatched characters
                // in the encrypted buffer that we cannot convert to string with UTF8.
                var strEncrypted = CryptographicBuffer.EncodeToBase64String(buffEncrypted);
                
                return strEncrypted;
            }
            catch (Exception ex)
            {
                return "";
            }
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
