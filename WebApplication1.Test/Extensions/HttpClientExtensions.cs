using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using WebApplication1.Requests;

namespace Tests.Extensions
{
    public static class HttpClientExtensions
    {

        public static async Task<string> AuthorizeClient(this HttpClient client, string nicknameOrEmail, string password)
        {
            var cookieCollection = new CookieCollection();
            const string uri = "api/v1/login";
            var request = new LoginRequest
            {
                NicknameOrEmail = nicknameOrEmail,
                Password = password
            };

            var response = await client.PostAsJsonAsync("api/v1/login", request);
            var cookie = response.Headers.FirstOrDefault(h => h.Key.Equals("Set-Cookie")).Value;
            client.DefaultRequestHeaders.Add("Cookie", cookie);
            return cookie.First();
        }
    }
}