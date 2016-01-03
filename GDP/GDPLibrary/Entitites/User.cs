using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GDPLibrary.Entitites
{
    public class User
    {
        private int idUser;
        private string username;
        private string password;
        private string token;

        [DataMember]
        public int IdUser
        {
            get { return idUser; }
            set { idUser = value; }
        }
        [DataMember]
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        [DataMember]
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

    }
}
