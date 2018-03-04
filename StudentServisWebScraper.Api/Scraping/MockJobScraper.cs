using System.Collections.Generic;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using StudentServisWebScraper.Api.Data;
using System.Linq;

namespace StudentServisWebScraper.Api.Scraping
{
    /// <summary>
    /// Class used to simulate constant job additions and removals, by using the already existing jobs
    /// in the database. 
    /// </summary>
    public class MockJobScraper : JobScraper
    {
        private const string TextPrefix = "Generirani oglas";
        private string[] JobTexts =
        {
            "traži se pizzaiolo za peć pizze bar 7 dana tjedno i 16 sati dnevno; plaća 20 kn po satu, javiti se na djubre@restoran.hr ili na 091/535-6658",
            "zapošljavamo drvodjelca koji može nacjepat 40m3 drva u što kraćem roku, plaćamo 35,00 kuna satnicu i gablec, nudimo i piljevinu. zvati na 01/6068698",
            "traže se studenti za kopanje tunela kao alternative pelješkom mostu. satnica dobra, zvati na 091 6823 555",
            "studenticu tražim za čuvanje moje dece, plaćam 15 kuna po satu javite se na ana@hrana.hr",
            "tražimo više studenata za rad u modernoj trgovini dijelovima automobila, satnica 28,00kn"
        };

        private string[] JobCategories =
        {
            "Fizički poslovi",
            "Razno",
            "Prodaja",
            "Poslovi u turizmu",
            "Rad u skladištima"
        };

        private Random rnd;
        private StudentServisWebScraperDataContext context;

        public MockJobScraper(ScraperConfiguration configuration, StudentServisWebScraperDataContext context)
            : base(configuration)
        {
            this.context = context;
            rnd = new Random();
        }

        public override ICollection<JobOfferInfo> Scrape()
        {
            List<JobOfferInfo> jobs = GetExistingJobs();

            // in 70% of cases, return nothing new
            if (Chance(70.Percent()))
            {
                return jobs;
            }

            // after, 50% chance of some jobs removed
            if (Chance(50.Percent()))
            {
                int countToRemove = rnd.Next(1, 4);
                for (int i = 0; i < countToRemove; i++)
                {
                    RemoveSingle(jobs);
                }

            }

            // after, 70% chance of some jobs added
            if (Chance(70.Percent()))
            {
                int countToAdd = rnd.Next(1, 4);
                for (int i = 0; i < countToAdd; i++)
                {
                    AddSingle(jobs);
                }
            }

            return jobs;
        }

        private List<JobOfferInfo> GetExistingJobs()
        {
            return context.JobOffers
                .Where(jo => !jo.DateRemoved.HasValue)
                .Select(jo => new JobOfferInfo(jo.Text, jo.Category))
                .ToList();
        }

        private int Percentage()
        {
            return rnd.Next(1, 101);
        }

        private bool Chance(int percentage)
        {
            return Percentage() >= percentage;
        }

        private void RemoveSingle(List<JobOfferInfo> jobs)
        {
            int indexToRemove = rnd.Next(0, jobs.Count);
            jobs.RemoveAt(indexToRemove);
        }

        private void AddSingle(List<JobOfferInfo> jobs)
        {
            string text = string.Format("{0}/ {1}; {2}",
                rnd.Next(1, 500) + 100000,
                TextPrefix,
                JobTexts[rnd.Next(0, JobTexts.Length)]);
            string category = JobCategories[rnd.Next(0, JobCategories.Length)];

            jobs.Add(new JobOfferInfo(text, category));
        }
    }

    public static class ReadabilityUtils
    {
        public static int Percent(this int n) => n;
    }
}
