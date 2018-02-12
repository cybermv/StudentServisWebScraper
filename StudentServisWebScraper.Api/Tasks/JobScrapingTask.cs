using Microsoft.Extensions.Logging;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Scraping;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        public JobScrapingTask(JobScraper scraper, JobOfferParser parser, JobOfferDataManager storage, ILogger<JobScrapingTask> logger)
        {
            this.Scraper = scraper;
            this.Parser = parser;
            this.Storage = storage;
            this.Logger = logger;
            this.ScrapingId = Guid.NewGuid();
        }

        public JobScraper Scraper { get; private set; }

        public JobOfferParser Parser { get; private set; }

        public JobOfferDataManager Storage { get; private set; }

        public ILogger<JobScrapingTask> Logger { get; set; }

        public Guid ScrapingId { get; set; }

        public void Execute()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.Logger.LogInformation($"Scraping process Id: {this.ScrapingId} - started scraping # T+0ms");

            ICollection<JobOfferInfo> scrapedJobs = this.Scraper.Scrape();

            this.Logger.LogInformation($"Scraping process Id: {this.ScrapingId} - scraping done, parsing, T+{stopwatch.ElapsedMilliseconds}ms");

            IEnumerable<JobOffer> parsedJobs = this.Parser.Parse(scrapedJobs);

            this.Logger.LogInformation($"Scraping process Id: {this.ScrapingId} - parsing done, storing, T+{stopwatch.ElapsedMilliseconds}ms");

            this.Storage.Store(parsedJobs);

            this.Logger.LogInformation($"Scraping process Id: {this.ScrapingId} - storage done, task finished, T+{stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
