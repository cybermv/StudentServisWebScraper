using System;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Models
{
    public class AdministrationViewModel
    {
        public int TotalActiveCount { get; set; }

        public int TotalDeletedCount { get; set; }

        public int TotalUnparsedCount { get; set; }

        public List<JobByCategoryStatistics> ByCategoryStatistics { get; set; }

        public decimal AverageHourlyPay { get; set; }

        public double AverageNewJobsPerDay { get; set; }

        public double AverageDeletedJobsPerDay { get; set; }

        public double AverageJobParsingSuccesses { get; set; }
    }
    
    public class JobByCategoryStatistics
    {
        public string Category { get; set; }

        public int ActiveCount { get; set; }

        public int DeletedCount { get; set; }

        public decimal AverageHourlyPay { get; set; }
    }
}
