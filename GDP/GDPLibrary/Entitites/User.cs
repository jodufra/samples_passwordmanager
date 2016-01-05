using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GDPLibrary.Entities
{
    [DataContract]
    public class User
    {

        private int idUser;
        private string username;
        private string password;
        private string salt;
        private string token;
        private string certSubject;
        private string certIssuer;
        private string certThumbprint;
        private string certSerialNumber;
        private DateTime? certValidFrom;
        private DateTime? certValidTo;

        public string Password { get { return password; } set { password = value; } }
        public string Token { get { return token; } set { token = value; } }
        public string Salt { get { return salt; } set { salt = value; } }

        [DataMember]
        public int IdUser { get { return idUser; } set { idUser = value; } }
        [DataMember]
        public string Username { get { return username; } set { username = value; } }
        [DataMember]
        public string CertSubject { get { return certSubject; } set { certSubject = value; } }
        [DataMember]
        public string CertIssuer { get { return certIssuer; } set { certIssuer = value; } }
        [DataMember]
        public string CertThumbprint { get { return certThumbprint; } set { certThumbprint = value; } }
        [DataMember]
        public string CertSerialNumber { get { return certSerialNumber; } set { certSerialNumber = value; } }
        [DataMember]
        public DateTime? CertValidFrom { get { return certValidFrom; } set { certValidFrom = value; } }
        [DataMember]
        public DateTime? CertValidTo { get { return certValidTo; } set { certValidTo = value; } }

        public User()
        {
            CertValidFrom = null;
            CertValidTo = null;
        }
    }
}