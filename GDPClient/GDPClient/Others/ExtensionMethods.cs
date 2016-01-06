using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows;
using Windows.Web.Http.Headers;

public static class ExtensionMethods
{
    // Usage: string query = (new { login = new { Username = "admin", Password = "admin" } }).ToQueryString();
    public static string ToQueryString(this object obj, string name = null)
    {
        bool hasName = !String.IsNullOrEmpty(name);
        var properties = from p in obj.GetType().GetProperties()
                         where p.GetValue(obj, null) != null
                         select (hasName ? name + "[" + p.Name + "]" : p.Name) + "=" + System.Net.WebUtility.UrlEncode(p.GetValue(obj, null).ToString());
        return String.Join("&", properties.ToArray());
    }
}
