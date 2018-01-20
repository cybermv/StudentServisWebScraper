using StudentServisWebScraper.Api.Scraping.Models;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Configuration class that contains specific data used by the scraper
    /// Use the scraper.json file to configure the scraping process
    /// </summary>
    public class ScraperConfiguration
    {
        public string RootUrl { get; set; }

        public string JobOfferUrl { get; set; }

        public int ScrapingIntervalMinutes { get; set; }

        public CategoryInfo[] Categories { get; set; }
    }
}