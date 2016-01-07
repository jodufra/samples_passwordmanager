using Dapper;
using GDPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDPWebApi.Repositories
{
    public class RecordRepository : BaseRepository
    {

        private User User { get { return Security.Session.Current; } }

        public List<Record> Get()
        {
            using (var conn = OpenConnection())
            {
                var query = @"SELECT * FROM Record WHERE IdUser = @IdUser";
                return SqlMapper.Query<Record>(conn, query, new { IdUser = User.IdUser }, null, true, null, null).ToList();
            }
        }

        public Record Get(int idRecord)
        {
            using (var conn = OpenConnection())
            {
                var query = @"SELECT * FROM Record WHERE IdRecord = @IdRecord AND IdUser = @IdUser";
                return SqlMapper.Query<Record>(conn, query, new { IdRecord = idRecord, IdUser = User.IdUser }, null, true, null, null).FirstOrDefault();
            }
        }

        public List<String> Save(Record record)
        {
            List<String> errors = new List<string>();

            if (record.IdCategory < 1)
                errors.Add("Category is required");

            if (record.IdUser < 1)
                errors.Add("User is required");

            if (record.Entry == null || record.Entry.Length <= 1)
                errors.Add("Entry is invalid");

            if (!errors.Any())
            {
                if (record.IdRecord == 0)
                {
                    using (var conn = OpenConnection())
                    {
                        var query = "INSERT INTO \"Record\" (IdCategory,IdUser,Entry) " +
                                    "VALUES (@IdCategory,@IdUser,@Entry);";
                        try
                        {
                            SqlMapper.Query(conn, query, new
                            {
                                IdCategory = record.IdCategory,
                                IdUser = User.IdUser,
                                Entry = record.Entry
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
                    using (var conn = OpenConnection())
                    {
                        var query = "UPDATE \"Record\" " +
                                    "SET IdCategory=@IdCategory,Entry=@Entry " +
                                    "WHERE IdRecord=@IdRecord AND IdUser=@IdUser;";
                        try
                        {
                            SqlMapper.Query(conn, query, new
                            {
                                IdCategory = record.IdCategory,
                                Entry = record.Entry,
                                IdRecord = record.IdRecord,
                                IdUser = User.IdUser
                            }, null, true, null, null).SingleOrDefault();
                        }
                        catch
                        {
                            errors.Add("Unexpected error");
                        }
                    }
                }
            }
            return errors;
        }

        public void Remove(int idRecord)
        {
            using (var conn = OpenConnection())
            {
                var query = "DELETE FROM \"Record\" " +
                            "WHERE IdRecord=@IdRecord AND IdUser=@IdUser;";
                SqlMapper.Query(conn, query, new
                {
                    IdRecord = idRecord,
                    IdUser = User.IdUser
                }, null, true, null, null).SingleOrDefault();
            }
        }

    }
}