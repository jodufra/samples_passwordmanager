using GDPService.Entities;
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
        string Login(string username, string password);

        [OperationContract]
        List<Category> GetCategories();

        [OperationContract]
        Category GetCategory(int idCategory);
    }
}
