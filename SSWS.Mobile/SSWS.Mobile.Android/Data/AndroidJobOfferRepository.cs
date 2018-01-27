using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
            IJobOfferRepository repo = new MockJobOfferRepository();
            return await repo.GetJobOffers(changedAfter, categoryIds, minHourlyPay);
        }
    }
}