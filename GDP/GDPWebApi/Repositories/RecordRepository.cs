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
            throw new NotImplementedException();
        }

        public void Remove(int idRecord)
        {
            throw new NotImplementedException();
        }

    }
}