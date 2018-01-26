using System;

namespace SSWS.Mobile.Models
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

        public bool IsHourlyPayDefined => HourlyPay.HasValue;

        public string TextShortened => Text.Substring(0, Math.Min(60, Text.Length)) + "...";

        public string Caption => HourlyPay.HasValue ? HourlyPay.Value + " kn/h" : "";
    }
}
