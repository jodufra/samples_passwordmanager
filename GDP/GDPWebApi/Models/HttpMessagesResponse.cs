using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GDPWebApi.Models
{
    public class HttpMessagesResponse<T>
    {
        public bool Error { get; set; }
        public IEnumerable<T> Messages { get; set; }
    }
}