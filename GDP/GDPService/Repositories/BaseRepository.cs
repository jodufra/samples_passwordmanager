using GDPLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GDPService.Repositories
{
    public abstract class BaseRepository
    {
        protected IDbConnection OpenConnection()
        {
            var c = new SqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
            c.Open();
            return c;
        }
    }
}