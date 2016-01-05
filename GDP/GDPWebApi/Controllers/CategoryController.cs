using GDPLibrary.Entities;
using GDPWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDPWebApi.Controllers
{
    [Authorize]
    public class CategoryController : ApiController
    {
        // GET: api/Category
        public IEnumerable<Category> Get()
        {
            return (new CategoryRepository()).Get();
        }
    }
}
