using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GDPLibrary.Utils
{
    public static class Serializer
    {
        public static byte[] ToByteArray(Object source)
        {
            DataContractSerializer serializer = new DataContractSerializer(source.GetType());

            byte[] result;
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                result = ms.ToArray();
            }
            return result;
        }

        public static Object FromByteArray(byte[] source, Type type)
        {
            DataContractSerializer serializer = new DataContractSerializer(type);
            object result;
            using (var ms = new MemoryStream(source))
            {
                result = serializer.ReadObject(ms);
            }
            return result;
        }

    }
}
