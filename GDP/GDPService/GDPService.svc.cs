﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GDPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GDPService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GDPService.svc or GDPService.svc.cs at the Solution Explorer and start debugging.
    public class GDPService : IGDPService
    {
        public string Login()
        {
            return "work done";
        }
    }
}
