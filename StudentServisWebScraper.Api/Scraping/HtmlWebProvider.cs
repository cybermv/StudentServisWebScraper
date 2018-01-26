using HtmlAgilityPack;
using System.Text;

namespace StudentServisWebScraper.Api.Scraping
{
    public class HtmlWebProvider
    {
        public static HtmlWeb GetInstance()
        {
            return new HtmlWeb
            {
                AutoDetectEncoding = false,
                UsingCache = false,
                OverrideEncoding = Encoding.UTF8
            };
        }
    }
}
