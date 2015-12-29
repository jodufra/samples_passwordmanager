using Dapper;
using GDPService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDPService.Repositories
{
    public class UserRepository : BaseRepository
    {

        public User Get(string username, string password)
        {
            using (var conn = OpenConnection())
            {
                var query = "SELECT * FROM \"User\" WHERE Username = @Username AND Password = @Password";
                return SqlMapper.Query<User>(conn, query, new { Username = username, Password = password }, null, true, null, null).FirstOrDefault();
            }
        }
    }
}