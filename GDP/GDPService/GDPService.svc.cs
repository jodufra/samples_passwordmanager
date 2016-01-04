using GDPLibrary.Entities;
using GDPService.Repositories;
using System;
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
        private CategoryRepository CategoryRepository;
        private UserRepository UserRepository;

        public GDPService()
        {
            CategoryRepository = new CategoryRepository();
            UserRepository = new UserRepository();
        }

        #region Users and Auths
        public User Login(string username, string password)
        {
            return UserRepository.Get(username, password);
        }

        public User LoginWithCertificate()
        {
            throw new NotImplementedException();
        }

        public List<string> RegisterUser(User user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Categories
        public List<Category> GetCategories()
        {
            return CategoryRepository.Get();
        }
        #endregion

        #region Records
        public List<Record> GetRecords(string token)
        {
            throw new NotImplementedException();
        }
        #endregion



    }
}
