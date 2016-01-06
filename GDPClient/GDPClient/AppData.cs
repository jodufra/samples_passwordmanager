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
        public static AppData Instance { get { return instance ?? (instance = new AppData()); } }
        public User User { get; set; }
        public List<Category> Categories { get; set; }
        private AppData() {

        }
        

    }
}


