using System.Collections.Generic;
using HtmlAgilityPack;
using StudentServisWebScraper.Api.Scraping.Models;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Class used to start the extraction from the initial page - uses JobOfferUrl
    /// to fetch the page and tries extracting as much job offers as possible
    /// </summary>
    public class GeneralJobOfferScraper : IJobOfferScraper
    {
        public ScraperConfiguration Configuration { get; private set; }

        public GeneralJobOfferScraper(ScraperConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public ICollection<JobOffer> ScrapeJobs(HtmlDocument document)
        {
            return new JobOffer[0];
        }
    }
}
