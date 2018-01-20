using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentServisWebScraper.Api.Data
{
    /// <summary>
    /// Class used to store new <see cref="JobOffer"/> entities into the
    /// database, and to mark old and removed offers as removed
    /// </summary>
    public class JobOfferDataManager
    {
        public JobOfferDataManager(StudentServisWebScraperDataContext context)
        {
            this.DataContext = context;
            this.Now = DateTime.UtcNow;
        }

        public DateTime Now { get; set; }

        public StudentServisWebScraperDataContext DataContext { get; set; }

        public void Store(IEnumerable<JobOffer> offers)
        {
            List<JobOffer> existingOffers = this.DataContext.JobOffers
                .Where(jo => !jo.DateRemoved.HasValue)
                .ToList();

            foreach (JobOffer offer in offers)
            {
                // the job offer is in the database and on the website - update it
                if (existingOffers.Exists(eo => eo.Code == offer.Code && eo.Category == offer.Category))
                {
                    JobOffer existing = existingOffers.Single(eo => eo.Code == offer.Code && eo.Category == offer.Category);
                    existing.DateLastChanged = this.Now;
                    existing.Text = offer.Text;
                    existing.ContactEmail = offer.ContactEmail;
                    existing.ContactPhone = offer.ContactPhone;
                    existing.HourlyPay = offer.HourlyPay;

                    this.DataContext.Update(existing);
                    existingOffers.Remove(existing);
                }
                // the offer doesn't exist in the database - add it
                else
                {
                    this.DataContext.Add(offer);
                }
            }

            foreach (JobOffer expired in existingOffers)
            {
                // every offer in database but not on the website is marked as removed
                expired.DateRemoved = this.Now;
                this.DataContext.Update(expired);
            }

            this.DataContext.SaveChanges();
        }
    }
}
