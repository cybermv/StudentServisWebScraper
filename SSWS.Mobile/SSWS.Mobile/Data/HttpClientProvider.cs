using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SSWS.Mobile.Data
{
    public static class HttpClientProvider
    {
        public static HttpClient GetClient(string baseUrl)
        {
            HttpClient instance = new HttpClient { BaseAddress = new Uri(baseUrl) };
            instance.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                Private = true
            };
            return instance;
        }
    }
}
