using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GDPClient
{
    public class AppData
    {
        private static AppData instance;
        public User User { get; set; }
        private AppData() {

        }

        public static AppData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppData();
                }
                return instance;
            }
        }
    }
}


