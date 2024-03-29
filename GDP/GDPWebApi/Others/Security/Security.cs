﻿using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Security.Principal;
using System.Linq;
using System.Web.Configuration;
using GDPLibrary.Entities;

namespace GDPWebApi.Security
{
    internal static class Settings
    {
        public static String ContextUserKey
        {
            get { return "GDP_USER"; }
        }
    }

    public static class Session
    {
        public static User Current { get { return (User)HttpContext.Current.Items[Settings.ContextUserKey]; } }

        public static Boolean IsAuthenticated { get { return HttpContext.Current.Items[Settings.ContextUserKey] != null; } }

        public static void SignIn(User user, Boolean RememberMe)
        {
            FormsAuthentication.SetAuthCookie(user.IdUser.ToString(), RememberMe);
            HttpContext.Current.Items[Settings.ContextUserKey] = user;
        }

        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}