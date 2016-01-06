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
        private Entry parsedEntry;

        [DataMember]
        public int IdRecord { get { return idRecord; } set { idRecord = value; } }
        [DataMember]
        public int IdCategory { get { return idCategory; } set { idCategory = value; } }
        [DataMember]
        public int IdUser { get { return idUser; } set { idUser = value; } }
        [DataMember]
        public byte[] Entry { get { return entry; } set { entry = value; } }

        public Entry ParseEntry(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            if (entry == null || entry.Length == 0)
                throw new Exception("Null or empty entries cant be parsed.");

            return parsedEntry ?? (parsedEntry = GDPLibrary.Entities.Entry.Parse(entry, user));
        }

        public void CryptEntry(User user)
        {
            if (user == null)
                throw new ArgumentNullException("User");

            if (parsedEntry == null)
                throw new Exception("Null ParsedEntry cant be crypted.");

            entry = GDPLibrary.Entities.Entry.Crypt(parsedEntry, user);
        }
    }
}
