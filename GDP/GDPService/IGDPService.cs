using GDPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GDPService
{
    [ServiceContract]
    public interface IGDPService
    {
        [OperationContract]
        User BasicAuth(string username, string password);

        [OperationContract]
        List<Category> GetCategories();
    }
}
