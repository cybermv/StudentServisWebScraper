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
        public HttpClientJobOffersRepository() { }

        public async Task<List<JobModel>> GetJobOffers(
            DateTime? addedAfter = null,
            int[] categoryIds = null,
            decimal? minHourlyPay = null)
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

        private HttpClient GetClient()
        {
            var client = new HttpClient
            {

                // TODO: load from settings
                BaseAddress = new Uri(@"http://ssws.azurewebsites.net/")
            };

            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true,
                Private = true
            };

            return client;
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
