using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GDPLibrary.Entities
{
    [DataContract]
    public class Record
    {
        private int idRecord;
        private int idCategory;
        private int idUser;
        private byte[] entry;

        [DataMember]
        public int IdRecord { get { return idRecord; } set { idRecord = value; } }
        [DataMember]
        public int IdCategory { get { return idCategory; } set { idCategory = value; } }
        [DataMember]
        public int IdUser { get { return idUser; } set { idUser = value; } }
        [DataMember]
        public byte[] Entry { get { return entry; } set { entry = value; } }

    }
}
