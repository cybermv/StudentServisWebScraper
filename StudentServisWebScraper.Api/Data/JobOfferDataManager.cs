using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StudentServisWebScraper.Api.Scraping;
using System;
using System.Collections.Generic;
using System.Data;
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
            using (IDbContextTransaction transaction = this.DataContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                StoreInternal(offers);

                try
                {
                    this.DataContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new ScrapingException($"Storing of new job offers failed; exception message: {ex.Message}");
                }
            }
        }

        private void StoreInternal(IEnumerable<JobOffer> offers)
        {
            // all currently valid offers
            List<JobOffer> existingOffers = this.DataContext.JobOffers
                .Where(jo => !jo.DateRemoved.HasValue)
                .ToList();

            Func<JobOffer, JobOffer, bool> comparer = JobOfferEqualityComparer.Comparer();

            foreach (JobOffer offer in offers)
            {
                int count = existingOffers.Count(eo => JobOfferEqualityComparer.Comparer()(eo, offer));

                // the offer is in the database and on the website - update the date
                if (count == 1)
                {
                    JobOffer existing = existingOffers.Single(eo => comparer(eo, offer));

                    existing.DateLastChanged = this.Now;

                    this.DataContext.Update(existing);
                    existingOffers.Remove(existing);
                }
                // the offer doesn't exist in the database - add it
                else if (count == 0)
                {
                    this.DataContext.Add(offer);
                }
                // something is fucked up!
                else
                {
                    IEnumerable<JobOffer> fuckedUpOffers = existingOffers.Where(eo => comparer(eo, offer));

                    foreach (JobOffer fuckedUpOffer in fuckedUpOffers)
                    {
                        // remove the fucked up ones
                        fuckedUpOffer.DateRemoved = this.Now;
                        this.DataContext.Update(fuckedUpOffer);
                    }
                    existingOffers.RemoveAll(eo => comparer(eo, offer));

                    // add the new one
                    this.DataContext.Add(offer);

                    // TODO: log this mistake
                }
            }

            // every existing offer in database but not found on the website is marked as removed
            foreach (JobOffer expired in existingOffers)
            {
                expired.DateRemoved = this.Now;
                this.DataContext.Update(expired);
            }
        }
    }
}
