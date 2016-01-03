using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using GDPLibrary.Entities;

namespace GDPService.Repositories
{
    public class CategoryRepository : BaseRepository
    {

        public List<Category> Get()
        {
            using (var conn = OpenConnection())
            {
                var query = @"SELECT * FROM Category";
                return SqlMapper.Query<Category>(conn, query).ToList();
            }
        }

        public Category Get(int idCategory)
        {
            using (var conn = OpenConnection())
            {
                var query = @"SELECT * FROM Category WHERE IdCategory = @IdCategory";
                return SqlMapper.Query<Category>(conn, query, new { IdCategory = idCategory }, null, true, null, null).FirstOrDefault();
            }
        }

    }
}