using HtmlAgilityPack;
using StudentServisWebScraper.Api.Scraping.Models;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Scraping
{
    public interface IJobOfferScraper
    {
        ICollection<JobOfferInfo> ScrapeJobs(HtmlDocument document);
    }
}
