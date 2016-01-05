using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GDPWebApi.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
