using Microsoft.AspNetCore.Mvc;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Scraping;
using StudentServisWebScraper.Api.Scraping.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentServisWebScraper.Api.Controllers
{
    [Route("api/[controller]")]
    public class JobsController : Controller
    {
        public JobsController(ScraperConfiguration configuration, StudentServisWebScraperDataContext context)
        {
            this.ScraperConfiguration = configuration;
            this.DataContext = context;
        }

        public ScraperConfiguration ScraperConfiguration { get; set; }

        public StudentServisWebScraperDataContext DataContext { get; set; }

        // GET: api/jobs
        [HttpGet]
        public List<JobOffer> GetJobs()
        {
            List<JobOffer> jobs = this.DataContext.JobOffers.ToList();

            return jobs;
        }

        // GET: api/jobs/categories
        [HttpGet("categories")]
        public List<CategoryInfo> GetCategories()
        {
            return this.ScraperConfiguration.Categories.ToList();
        }

        // GET: api/jobs/{jobGuid}
        [HttpGet("{jobGuid}")]
        public ActionResult GetJobs(Guid jobGuid)
        {
            JobOffer job = this.DataContext.JobOffers.Find(jobGuid);

            if (job != null)
            {
                return Ok(job);
            }
            else
            {
                return NotFound();
            }
        }

        // GET: api/jobs/category/{jobCategory}
        [HttpGet("category/{jobCategory}")]
        public List<JobOffer> GetJobsByCategory(string jobCategory)
        {
            List<JobOffer> jobs = this.DataContext.JobOffers
                .Where(j => j.Category.Equals(jobCategory, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return jobs;
        }
    }
}
