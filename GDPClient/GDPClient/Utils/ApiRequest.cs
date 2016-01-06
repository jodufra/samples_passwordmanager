using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace GDPClient.Others
{
    public static class ApiRequest
    {
        public static async Task<HttpResponseMessage> MakeRequest(String url, HttpMethod method, String query = null)
        {
            var completeUrl = !String.IsNullOrEmpty(query) ? String.Join("?", url, query) : url;
            using (var httpClient = new HttpClient())
            {
                Uri uri = new Uri(completeUrl);
                HttpRequestMessage mSent = new HttpRequestMessage(HttpMethod.Post, uri);
                return await httpClient.SendRequestAsync(mSent, HttpCompletionOption.ResponseContentRead);
            }
        }
        
    }
}
