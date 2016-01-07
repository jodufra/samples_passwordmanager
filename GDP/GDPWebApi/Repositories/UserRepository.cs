using Dapper;
using GDPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDPWebApi.Repositories
{
    public class UserRepository : BaseRepository
    {

        public User Get(int idUser)
        {
            using (var conn = OpenConnection())
            {
                var query = "SELECT * FROM \"User\" WHERE IdUser = @IdUser";
                return SqlMapper.Query<User>(conn, query, new { IdUser = idUser }, null, true, null, null).FirstOrDefault();
            }
        }

        public User Get(string username)
        {
            using (var conn = OpenConnection())
            {
                var query = "SELECT * FROM \"User\" WHERE Username = @Username";
                return SqlMapper.Query<User>(conn, query, new { Username = username }, null, true, null, null).FirstOrDefault();
            }
        }

        public User Get(string username, string password)
        {
            using (var conn = OpenConnection())
            {
                var query = "SELECT * FROM \"User\" WHERE Username = @Username AND Password = @Password";
                return SqlMapper.Query<User>(conn, query, new { Username = username, Password = password }, null, true, null, null).FirstOrDefault();
            }
        }

        public User GetByCertThumbprint(String certThumbprint)
        {
            using (var conn = OpenConnection())
            {
                var query = "SELECT * FROM \"User\" WHERE CertThumbprint = @CertThumbprint";
                return SqlMapper.Query<User>(conn, query, new { CertThumbprint = certThumbprint }, null, true, null, null).FirstOrDefault();
            }
        }

        public List<String> Save(User user)
        {
            var errors = new List<String>();
            if (String.IsNullOrEmpty(user.Username))
                errors.Add("Invalid Username");
            else
            {
                var aux = Get(user.Username);
                if (aux != null && aux.IdUser != user.IdUser)
                    errors.Add("Username already exists");
            }
            if (String.IsNullOrEmpty(user.Password))
                errors.Add("Invalid Password");
            if (!String.IsNullOrEmpty(user.CertThumbprint))
            {
                var aux = GetByCertThumbprint(user.CertThumbprint);
                if (aux != null)
                    errors.Add("Certificate is Already being Used");
            }

            if (!errors.Any())
            {
                if (user.IdUser == 0)
                {
                    string salt;
                    user.Password = GDPLibrary.Utils.Security.GetSHA256SaltyPassword(user.Username, user.Password, out salt);
                    user.Salt = salt;
                    user.Token = GDPLibrary.Utils.Security.GetSHA256Hash(user.Username + user.Password + user.Salt);

                    using (var conn = OpenConnection())
                    {
                        var query = "INSERT INTO \"User\" (Password,Token,Salt,Username,CertSubject,CertIssuer,CertThumbprint,CertSerialNumber,CertValidFrom,CertValidTo) " +
                                    "VALUES (@Password,@Token,@Salt,@Username,@CertSubject,@CertIssuer,@CertThumbprint,@CertSerialNumber,@CertValidFrom,@CertValidTo);";
                        try
                        {
                            SqlMapper.Query(conn, query, new
                            {
                                Password = user.Password,
                                Token = user.Token,
                                Salt = user.Salt,
                                IdUser = user.IdUser,
                                Username = user.Username,
                                CertSubject = user.CertSubject,
                                CertIssuer = user.CertIssuer,
                                CertThumbprint = user.CertThumbprint,
                                CertSerialNumber = user.CertSerialNumber,
                                CertValidFrom = user.CertValidFrom,
                                CertValidTo = user.CertValidTo,
                            }, null, true, null, null).SingleOrDefault();
                        }
                        catch
                        {
                            errors.Add("Unexpected error");
                        }
                    }
                }
                else
                {

                }
            }

            return errors;
        }
    }
}