using System;

namespace StudentServisWebScraper.Api.Scraping.Models
{
    public class JobOffer
    {
        public JobOffer() : this(string.Empty)
        {

        }

        public JobOffer(string text)
        {
            this.Text = text;
        }

        public int Code { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateRemoved { get; set; }

        public string Text { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public decimal HourlyPay { get; set; }

        public string Category { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
