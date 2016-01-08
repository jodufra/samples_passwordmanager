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
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        public class UserUpdateIM
        {
            public int IdUser { get; set; }
            public String Username { get; set; }
            public String Password { get; set; }
            public string CertSubject { get; set; }
            public string CertIssuer { get; set; }
            public string CertThumbprint { get; set; }
            public string CertSerialNumber { get; set; }
            public DateTime? CertValidFrom { get; set; }
            public DateTime? CertValidTo { get; set; }

            public User Parse()
            {
                return new User()
                {
                    IdUser = IdUser,
                    Username = Username,
                    Password = Password,
                    CertSubject = CertSubject,
                    CertIssuer = CertIssuer,
                    CertThumbprint = CertThumbprint,
                    CertSerialNumber = CertSerialNumber,
                    CertValidFrom = CertValidFrom,
                    CertValidTo = CertValidTo
                };
            }
        }
        [HttpPost, Route("Update")]
        public HttpResponseMessage Update([FromUri]UserUpdateIM update)
        {
            if (update == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new List<String>() { "Information is missing." });
            if (update.IdUser < 1)
                return Request.CreateResponse(HttpStatusCode.BadRequest, new List<String>() { "Invalid User id." });
            var errors = (new UserRepository()).Save(update.Parse());
            return Request.CreateResponse(errors.Any() ? HttpStatusCode.BadRequest : HttpStatusCode.OK, errors.Any() ? errors : new List<String>() { "User updated with success." });
        }
    }
}
