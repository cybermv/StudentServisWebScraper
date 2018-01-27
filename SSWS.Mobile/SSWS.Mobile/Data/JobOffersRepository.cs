using System.Net;
using System.IO;
using System.Text;
using System.Net.Cache;
using SSWS.Mobile.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace SSWS.Mobile.Data
{
    public class JobOffersRepository
    {
        public JobOffersRepository()
        {
        }

        public async Task<List<JobModel>> GetJobOffers(DateTime? changedAfter = null, int[] categoryIds = null, decimal? minHourlyPay = null)
        {
            HttpClient client = GetClient();
            /*
            client.GetAsync()

            if (changedAfter.HasValue)
            {
                client.QueryString["changedAfter"] = changedAfter.ToString();
            }
            if (categoryIds != null && categoryIds.Length > 0)
            {
                client.QueryString["categoryIds"] = CategoriesToString(categoryIds);
            }
            if (minHourlyPay.HasValue)
            {
                client.QueryString["minHourlyPay"] = minHourlyPay.ToString();
            }
            */
            string data = "";
            try
            {
                data = await client.GetStringAsync("/api/jobs/filter");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                data = "[]";
            }

            

            List<JobModel> jobs = JsonConvert.DeserializeObject<List<JobModel>>(data);

            return jobs;
        }


        private HttpClient GetClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri(@"http://localhost/StudentServisWebScraper.Api/")
            };
        }

        private string CategoriesToString(int[] arr)
        {
            return "[" + arr.Select(x => x.ToString()).Aggregate((a, b) => a + ", " + b) + "]";
        }
    }
}
