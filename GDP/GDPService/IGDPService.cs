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
        // Users and Auth
        [OperationContract]
        User Login(string username, string password);

        [OperationContract]
        User LoginWithCertificate();

        [OperationContract]
        List<String> RegisterUser(User user);

        // Categories
        [OperationContract]
        List<Category> GetCategories();

        // Records
        [OperationContract]
        List<Record> GetRecords(String token);

    }
}
