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

namespace Utils
{
    public class Security
    {
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

        public static byte[] EncryptAES(byte[] source, string publicKey)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey");
            try
            {
                var str = Convert.ToBase64String(source);
                var toEncryptBuffer = CryptographicBuffer.DecodeFromBase64String(str);
                var aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                var symetricKey = aes.CreateSymmetricKey(GetMD5Hash(publicKey));
                var buffEncrypted = CryptographicEngine.Encrypt(symetricKey, toEncryptBuffer, null);
                byte[] result;
                CryptographicBuffer.CopyToByteArray(buffEncrypted, out result);
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public static byte[] DecryptAES(byte[] source, string publicKey)
        {
            if (source == null || source.Length == 0)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(publicKey))
                throw new ArgumentNullException("publicKey");
            try
            {
                var str = Convert.ToBase64String(source);
                IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(str);
                SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
                var symetricKey = aes.CreateSymmetricKey(GetMD5Hash(publicKey));
                var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);
                byte[] result;
                CryptographicBuffer.CopyToByteArray(buffDecrypted, out result);
                return result;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        
    }
}
