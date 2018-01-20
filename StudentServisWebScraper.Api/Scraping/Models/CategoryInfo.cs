namespace StudentServisWebScraper.Api.Scraping.Models
{
    public class CategoryInfo
    {
        public int Id { get; set; }

        public string FriendlyName { get; set; }

        public string ScrapeName { get; set; }

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}
