using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using SSWS.Mobile.Droid.Data;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidJobOfferRepository))]
namespace SSWS.Mobile.Droid.Data
{
    public class AndroidJobOfferRepository : IJobOfferRepository
    {
        public async Task<List<JobModel>> GetJobOffers(DateTime? changedAfter = null, int[] categoryIds = null, decimal? minHourlyPay = null)
        {
            // TODO: implement for real
            IJobOfferRepository repo = new HttpClientJobOffersRepository();
            return await repo.GetJobOffers(changedAfter, categoryIds, minHourlyPay);
        }
    }
}