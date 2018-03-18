using SSWS.Mobile.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;

namespace SSWS.Mobile.Data
{
    public class HttpClientJobOffersRepository : IJobOfferRepository
    {
        private UserSettings settings;
        private static HttpClient _httpClientInstance;

        public HttpClientJobOffersRepository()
        {
            this.settings = UserSettings.Load();
        }

        public async Task<List<JobModel>> GetJobOffers(
            DateTime? addedAfter = null,
            int[] categoryIds = null,
            decimal? minHourlyPay = null,
            bool excludeNonParsed = false)
        {
            NameValueCollection queryString = new NameValueCollection();

            if (addedAfter.HasValue)
            {
                queryString["addedAfter"] = addedAfter.Value.ToString("o");
            }
            if (categoryIds != null && categoryIds.Length > 0)
            {
                queryString["categoryIds"] = CategoriesToString(categoryIds);
            }
            if (minHourlyPay.HasValue)
            {
                queryString["minHourlyPay"] = minHourlyPay.ToString();
            }
            if (excludeNonParsed)
            {
                queryString["excludeNonParsed"] = excludeNonParsed.ToString();
            }
            

            string data = "[]";
            try
            {
                HttpClient client = GetClient();
                Uri requestUri = new Uri("/api/jobs/filter" + ToQueryString(queryString));
                data = await client.GetStringAsync(requestUri);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }

            List<JobModel> jobs = JsonConvert.DeserializeObject<List<JobModel>>(data);

            return jobs;
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            string data = "[]";
            try
            {
                HttpClient client = GetClient();
                Uri requestUri = new Uri("/api/jobs/categories/");
                data = await client.GetStringAsync(requestUri);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }

            List<CategoryModel> categories = JsonConvert.DeserializeObject<List<CategoryModel>>(data);

            return categories;
        }

        private HttpClient GetClient()
        {
            if (_httpClientInstance == null)
            {
                _httpClientInstance = new HttpClient { BaseAddress = new Uri(this.settings.ServiceUrl) };
                _httpClientInstance.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    Private = true
                };
            }

            return _httpClientInstance;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            string[] array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        private string CategoriesToString(int[] arr)
        {
            return "[" + arr.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b) + "]";
        }
    }
}
