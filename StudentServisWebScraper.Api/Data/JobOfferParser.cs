using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StudentServisWebScraper.Api.Data
{
    /// <summary>
    /// Class used to parse data from given <see cref="JobOfferInfo"/> into
    /// <see cref="JobOffer"/> entities with easily accesible data
    /// </summary>
    public class JobOfferParser
    {
        public JobOfferParser()
        {
            this.Now = DateTime.UtcNow;
        }

        private DateTime Now { get; set; }

        public IEnumerable<JobOffer> Parse(IEnumerable<JobOfferInfo> jobs)
        {
            foreach (JobOfferInfo job in jobs)
            {
                yield return ParseSingle(job);
            }
        }

        public JobOffer ParseSingle(JobOfferInfo job)
        {
            return new JobOffer
            {
                Id = Guid.Empty,
                Code = job.Code,
                Text = job.Text,
                Category = job.Category,
                DateAdded = this.Now,
                DateLastChanged = null,
                DateRemoved = null,
                ContactEmail = ExtractEmail(job.Text),
                ContactPhone = ExtractPhone(job.Text),
                HourlyPay = ExtractHourlyRate(job.Text)
            };
        }

        private string ExtractEmail(string text)
        {
            Match matchedEmail = Regex.Match(
                text,
                @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matchedEmail.Success ? matchedEmail.Value : null;
        }

        private string ExtractPhone(string text)
        {
            Match matchedPhone = Regex.Match(
                text,
                @"(\d{1,4}(\/|-| )\d{3,4}(\/|-| )?(\d{2,4})?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matchedPhone.Success ? matchedPhone.Value : null;
        }

        private decimal? ExtractHourlyRate(string text)
        {
            Match matchedHourlyRate = Regex.Match(
                text,
                @"(\d+(\,\d{1,2})? *(kn|kuna))",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!matchedHourlyRate.Success)
                return null;

            return decimal.Parse(Regex.Replace(
                matchedHourlyRate.Value,
                "(kn|kuna)",
                "",
                RegexOptions.Compiled | RegexOptions.IgnoreCase));
        }
    }
}
