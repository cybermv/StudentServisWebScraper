using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSWS.Mobile.Data
{
    public interface IJobOfferRepository
    {
        Task<List<JobModel>> GetJobOffers(
            DateTime? changedAfter = null,
            int[] categoryIds = null,
            decimal? minHourlyPay = null);
    }
}
