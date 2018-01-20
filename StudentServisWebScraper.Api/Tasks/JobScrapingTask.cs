using Microsoft.Extensions.DependencyInjection;
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
        public JobScrapingTask(JobScraper scraper, JobOfferParser parser, object storage)
        {
            this.Scraper = scraper;
            this.Parser = parser;
            this.Storage = storage;
        }

        public JobScraper Scraper { get; private set; }

        public JobOfferParser Parser { get; private set; }

        public object Storage { get; private set; }

        public void Execute()
        {
            ICollection<JobOfferInfo> scrapedJobs = this.Scraper.Scrape();

            IEnumerable<JobOffer> parsedJobs = this.Parser.Parse(scrapedJobs);

            // TODO: diff the entries in the database
        }

        public static void CreateAndExecute(IServiceProvider services)
        {
            JobScrapingTask task = services.GetService<JobScrapingTask>();

            task.Execute();
        }
    }
}
