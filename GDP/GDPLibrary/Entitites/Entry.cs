using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDPLibrary.Entities
{
    public class Entry
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Url { get; set; }


        public static Entry Parse(User user, byte[] stream)
        {
            return null;
        }

        public static byte[] Crypt(User user, Entry entry)
        {
            return null;
        }

    }
}
