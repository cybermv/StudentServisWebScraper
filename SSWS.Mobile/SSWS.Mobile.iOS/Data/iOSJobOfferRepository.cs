﻿using System;
using System.Collections.Generic;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using System.Threading.Tasks;
using SSWS.Mobile.iOS.Data;

[assembly: Xamarin.Forms.Dependency(typeof(iOSJobOfferRepository))]
namespace SSWS.Mobile.iOS.Data
{
    public class iOSJobOfferRepository : IJobOfferRepository
    {
        public async Task<List<JobModel>> GetJobOffers(DateTime? changedAfter = null, int[] categoryIds = null, decimal? minHourlyPay = null)
        {
            // TODO: implement for real
            IJobOfferRepository repo = new HttpClientJobOffersRepository();
            return await repo.GetJobOffers(changedAfter, categoryIds, minHourlyPay);
        }
        
        public Task<List<CategoryModel>> GetCategories()
        {
            // TODO: implement for real
            IJobOfferRepository repo = new HttpClientJobOffersRepository();
            return repo.GetCategories();
        }
    }
}