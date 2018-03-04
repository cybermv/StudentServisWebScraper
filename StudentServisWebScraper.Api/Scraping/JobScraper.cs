using StudentServisWebScraper.Api.Scraping.Models;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Class used to start the scraping process
    /// </summary>
    public abstract class JobScraper
    {
        public ScraperConfiguration Configuration { get; private set; }

        public JobScraper(ScraperConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Scrapes job offers from a source
        /// </summary>
        /// <returns>All found jobs</returns>
        public abstract ICollection<JobOfferInfo> Scrape();
    }
}
