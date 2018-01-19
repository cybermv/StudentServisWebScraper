using System;
using System.Text.RegularExpressions;

namespace StudentServisWebScraper.Api.Scraping.Models
{
    public class JobOfferInfo
    {
        public JobOfferInfo(string text, string category)
        {
            this.Text = text;
            this.Category = category;

            Match codeMatch = Regex.Match(text, @"^\d+");
            if(codeMatch == null)
            {
                throw new ArgumentException("Job offer text doesn't have a code.");
            }

            this.Code = int.Parse(codeMatch.Value);
        }

        public string Text { get; set; }

        public string Category { get; set; }

        public int Code { get; set; }
        
        public override string ToString()
        {
            return Text;
        }
    }
}
