using HtmlAgilityPack;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Class used to start the scraping process
    /// </summary>
    public class JobScraper
    {
        public ScraperConfiguration Configuration { get; private set; }

        public JobScraper(ScraperConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Uses the <see cref="GeneralJobOfferScraper"/> and multiple <see cref="CategorisedJobOfferScraper"/>
        /// to scrape most job offers from the page specified in the configuration
        /// </summary>
        /// <returns>All found jobs</returns>
        public ICollection<JobOfferInfo> Scrape()
        {
            List<JobOfferInfo> foundJobs = new List<JobOfferInfo>();
            HtmlWeb web = HtmlWebProvider.GetInstance();
            Uri jobOfferUri = new Uri(this.Configuration.RootUrl + this.Configuration.JobOfferUrl);
            HtmlDocument document = null;

            try
            {
                document = web.Load(jobOfferUri);
            }
            catch
            {
                throw new ScrapingException(
                    $"Cannot load job offer page; cannot continue scraping.");
            }

            IJobOfferScraper generalScraper = new GeneralJobOfferScraper(this.Configuration);

            ICollection<JobOfferInfo> foundGeneralJobs = generalScraper.ScrapeJobs(document);
            foundJobs.AddRange(foundGeneralJobs);

            foreach (CategoryInfo category in this.Configuration.Categories)
            {
                IJobOfferScraper categoryScraper = new CategorisedJobOfferScraper(this.Configuration, category);
                ICollection<JobOfferInfo> foundCategorisedJobs = categoryScraper.ScrapeJobs(document);
                foundJobs.AddRange(foundCategorisedJobs);
            }

            return foundJobs;
        }
    }
}
