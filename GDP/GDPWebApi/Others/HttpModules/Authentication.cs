using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Security.Principal;
using System.Linq;
using System.Web.Configuration;
using GDPLibrary.Entities;
using GDPWebApi.Repositories;

namespace GDPWebApi.HttpModules
{
    public class Authentication : IHttpModule
    {
        public Authentication()
        {

        }

        public void Init(HttpApplication app)
        {
            app.AuthenticateRequest += new EventHandler(OnAuthenticateRequest);
        }

        void OnAuthenticateRequest(object sender, EventArgs args)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;

            if (System.IO.Path.GetExtension(context.Request.Url.LocalPath) != "")
                return;

            User user = null;
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                user = (new UserRepository()).Get(Convert.ToInt32(context.User.Identity.Name));
                if (user != null) HttpContext.Current.Items.Add(GDPWebApi.Security.Settings.UserCookieName, user);
            }
        }

        public void Dispose() { }
    }
}
