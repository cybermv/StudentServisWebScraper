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
                // the job offer is in the database and on the website - check if it needs an update
                // IMPORTANT - this assumes that the code and category combination is unique!
                //             it may be not true in the end but I don't see another way to do it
                if (existingOffers.Exists(eo => eo.Code == offer.Code && eo.Category == offer.Category))
                {
                    JobOffer existing = existingOffers.Single(eo => eo.Code == offer.Code && eo.Category == offer.Category);

                    // the text of the offer is changed, update the entry
                    if (existing.Text != offer.Text)
                    {
                        existing.DateLastChanged = this.Now;
                        existing.Text = offer.Text;
                        existing.ContactEmail = offer.ContactEmail;
                        existing.ContactPhone = offer.ContactPhone;
                        existing.HourlyPay = offer.HourlyPay;

                        this.DataContext.Update(existing);
                    }

                    // remove from the list because the item is processed
                    existingOffers.Remove(existing);
                }
                // the offer doesn't exist in the database - add it
                else
                {
                    this.DataContext.Add(offer);
                }
            }

            // every existing offer in database but not found on the website is marked as removed
            foreach (JobOffer expired in existingOffers)
            {
                expired.DateRemoved = this.Now;
                this.DataContext.Update(expired);
            }

            this.DataContext.SaveChanges();
        }
    }
}
