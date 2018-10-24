using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SSWS.Mobile.Data.Interfaces
{
    public interface IJobOfferRepository
    {
        Task<List<JobModel>> GetJobOffers(
            DateTime? changedAfter = null,
            int[] categoryIds = null,
            decimal? minHourlyPay = null,
            bool excludeNonParsed = false);

        Task<List<CategoryModel>> GetCategories();
    }
}