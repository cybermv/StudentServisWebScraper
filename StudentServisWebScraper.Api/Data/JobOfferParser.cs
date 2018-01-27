using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
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
                DateLastChanged = this.Now,
                DateRemoved = null,
                ContactEmail = ExtractEmail(job.Text),
                ContactPhone = ExtractPhone(job.Text),
                HourlyPay = ExtractHourlyRate(job.Text)
            };
        }

        /// <summary>
        /// Simple regex to try and parse the email
        /// </summary>
        /// <param name="text">Text of the job offer</param>
        /// <returns>Parsed email or null if it cannot be located</returns>
        private string ExtractEmail(string text)
        {
            Match matchedEmail = Regex.Match(
                text,
                @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matchedEmail.Success ? matchedEmail.Value : null;
        }

        /// <summary>
        /// Simple regex to try and parse the phone number
        /// </summary>
        /// <param name="text">Text of the job offer</param>
        /// <returns>Parsed phone number or null if it cannot be located</returns>
        private string ExtractPhone(string text)
        {
            Match matchedPhone = Regex.Match(
                text,
                @"(\d{1,4}(\/|-| )\d{3,4}(\/|-| )?(\d{2,4})?)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return matchedPhone.Success ? matchedPhone.Value : null;
        }

        /// <summary>
        /// Simple regex to try and parse the hourly rate
        /// </summary>
        /// <param name="text">Text of the job offer</param>
        /// <returns>Parsed hourly rate or null if it cannot be located</returns>
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
