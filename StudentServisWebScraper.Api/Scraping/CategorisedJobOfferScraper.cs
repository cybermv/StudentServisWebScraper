using HtmlAgilityPack;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Class used to parse a specific category page that includes job offers only
    /// for that category. First finds the needed link to the category page
    /// and then gets all jobs that it can
    /// </summary>
    public class CategorisedJobOfferScraper : IJobOfferScraper
    {
        public ScraperConfiguration Configuration { get; private set; }

        public CategoryInfo Category { get; private set; }

        public CategorisedJobOfferScraper(ScraperConfiguration configuration, CategoryInfo category)
        {
            this.Configuration = configuration;
            this.Category = category;
        }

        public ICollection<JobOfferInfo> ScrapeJobs(HtmlDocument document)
        {
            HtmlNode link = document.DocumentNode.Descendants()
                .Where(n => n.Name.Equals("a", StringComparison.OrdinalIgnoreCase) && n.InnerText.Contains(this.Category.ScrapeName))
                .SingleOrDefault();

            if (link == null)
            {
                throw new ScrapingException(
                    $"Cannot locate link for category '{this.Category.ScrapeName}' in the document.");
            }

            HtmlWeb web = new HtmlWeb();
            Uri navigationUri = new Uri(this.Configuration.RootUrl + link.Attributes["href"].Value);
            HtmlDocument navigatedDocument = web.Load(navigationUri);

            HtmlNode content = navigatedDocument.DocumentNode
                .SelectSingleNode(@"//div[@id='mainContent']//div[@class='content']");

            if (content == null)
            {
                throw new ScrapingException(
                    $"Cannot locate content for category '{this.Category.ScrapeName}'.");
            }

            List<JobOfferInfo> foundOffers = new List<JobOfferInfo>();

            using (StringReader sr = new StringReader(content.InnerText))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (!Regex.IsMatch(line, @"^(\d+ ?\/).*", RegexOptions.Multiline)) continue;

                    JobOfferInfo joi = new JobOfferInfo(line, this.Category.FriendlyName);
                    foundOffers.Add(joi);
                }
            }

            return foundOffers;
        }
    }
}
