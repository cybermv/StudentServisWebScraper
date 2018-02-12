using System;
using System.Collections.Generic;

namespace StudentServisWebScraper.Api.Data
{
    public class JobOfferEqualityComparer : IEqualityComparer<JobOffer>
    {
        private static JobOfferEqualityComparer _current = new JobOfferEqualityComparer();

        public IEqualityComparer<JobOffer> Current => _current;

        public bool Equals(JobOffer x, JobOffer y)
        {
            return Comparer()(x, y);
        }

        public int GetHashCode(JobOffer obj)
        {
            return obj.GetHashCode();
        }

        public static Func<JobOffer, JobOffer, bool> Comparer()
        {
            return (x, y) => x.UniqueText == y.UniqueText;
        }
    }
}
