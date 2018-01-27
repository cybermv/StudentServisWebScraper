using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using System.Threading.Tasks;
using SSWS.Mobile.iOS.Data;

[assembly: Xamarin.Forms.Dependency(typeof(iOSJobOfferRepository))]
namespace SSWS.Mobile.iOS.Data
{
    public class iOSJobOfferRepository : IJobOfferRepository
    {
        public Task<List<JobModel>> GetJobOffers(DateTime? changedAfter = null, int[] categoryIds = null, decimal? minHourlyPay = null)
        {
            // TODO: implement for real
            IJobOfferRepository repo = new MockJobOfferRepository();
            return await repo.GetJobOffers(changedAfter, categoryIds, minHourlyPay);
        }
    }
}