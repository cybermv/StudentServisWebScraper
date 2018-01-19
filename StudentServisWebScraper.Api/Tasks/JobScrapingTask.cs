using StudentServisWebScraper.Api.Scraping;
using StudentServisWebScraper.Api.Scraping.Models;
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
        public JobScraper Scraper { get; private set; }

        public object Storage { get; private set; }

        public void Execute()
        {
            ICollection<JobOfferInfo> scrapedJobs = this.Scraper.Scrape();


        }
    }
}
