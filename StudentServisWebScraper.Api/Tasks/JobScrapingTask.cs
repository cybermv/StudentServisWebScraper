using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Scraping;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Tasks
{
    /// <summary>
    /// Class that defines the repeating process of:
    /// - acquiring jobs from the job offers web site,
    /// - parsing and cleaning as much data as possible,
    /// - storing the job offers in a database and updating existing entries
    /// </summary>
    public class JobScrapingTask
    {
        /// <summary>
        /// TODO: find a smarter way to get dependencies
        /// </summary>
        public static IServiceProvider Provider { get; set; }

        public JobScrapingTask(JobScraper scraper, JobOfferParser parser, JobOfferDataManager storage)
        {
            this.Scraper = scraper;
            this.Parser = parser;
            this.Storage = storage;
        }

        public JobScraper Scraper { get; private set; }

        public JobOfferParser Parser { get; private set; }

        public JobOfferDataManager Storage { get; private set; }

        public void Execute()
        {
            ICollection<JobOfferInfo> scrapedJobs = this.Scraper.Scrape();

            IEnumerable<JobOffer> parsedJobs = this.Parser.Parse(scrapedJobs);

            this.Storage.Store(parsedJobs);
        }
    }
}
