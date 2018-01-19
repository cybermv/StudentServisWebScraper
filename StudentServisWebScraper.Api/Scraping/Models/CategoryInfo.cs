namespace StudentServisWebScraper.Api.Scraping.Models
{
    public class CategoryInfo
    {
        public string FriendlyName { get; set; }

        public string ScrapeName { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
