﻿using GDPLibrary.Entities;
using GDPWebApi.Models;
using GDPWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GDPWebApi.Controllers
{
    [RoutePrefix("api/Auth")]
    public class AuthController : ApiController
    {
        [HttpGet, Route("Hello")]
        public HttpResponseMessage Hello()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello");
        }

        public class AuthLoginIM
        {
            public String Username { get; set; }
            public String Password { get; set; }
            public Boolean RememberMe { get; set; }
        }
        [HttpPost, Route("Login")]
        public HttpResponseMessage Login([FromUri] AuthLoginIM signin)
        {
            if (signin == null || String.IsNullOrEmpty(signin.Username) || String.IsNullOrEmpty(signin.Password))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Username and password are missing.");

            var user = (new UserRepository()).Get(signin.Username);

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid username");

            if (user.Password != GDPLibrary.Utils.Security.GetSHA256SaltyHashFromTastlessHash(signin.Password, user.Salt))
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid password");

            Security.Session.SignIn(user, signin.RememberMe);

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [HttpPost, Route("LoginCertificate")]
        public HttpResponseMessage LoginCertificate()
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        public class AuthRegisterIM
        {
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
        [HttpPost, Route("Register")]
        public HttpResponseMessage Register([FromUri] AuthRegisterIM register)
        {
            if (register == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Username and password are missing.");

            var errors = (new UserRepository()).Save(register.Parse());

            var response = new HttpMessagesResponse<String>();
            response.Error = errors.Any();
            response.Messages = response.Error ? errors : new List<String>() { "User registered with success." };
            return Request.CreateResponse(response.Error ? HttpStatusCode.BadRequest : HttpStatusCode.OK, response);
        }

        [Authorize]
        [HttpPost, Route("Logout")]
        public HttpResponseMessage Logout()
        {
            if (Security.Session.IsAuthenticated)
                Security.Session.SignOut();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}