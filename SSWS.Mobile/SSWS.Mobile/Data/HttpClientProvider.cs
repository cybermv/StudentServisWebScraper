using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SSWS.Mobile.Data
{
    public static class HttpClientProvider
    {
        // TODO - move to config?
        private const string serviceUrl = "http://ssws.azurewebsites.net/";

        public static HttpClient GetClient()
        {
            HttpClient instance = new HttpClient { BaseAddress = new Uri(serviceUrl) };
            instance.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                Private = true
            };
            // TODO - add user agent string
            return instance;
        }
    }
}
