using System;

namespace StudentServisWebScraper.TestApplication.Models
{
    public class JobModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public int Code { get; set; }

        public string Category { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateLastChanged { get; set; }

        public DateTime? DateRemoved { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEmail { get; set; }

        public decimal? HourlyPay { get; set; }
    }
}
