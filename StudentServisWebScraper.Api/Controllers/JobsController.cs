using Microsoft.AspNetCore.Mvc;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.ModelBinding;
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
            List<JobOffer> jobs = this.DataContext.JobOffers
                .Where(j => !j.DateRemoved.HasValue)
                .OrderByDescending(j => j.DateLastChanged)
                .ToList();

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
        public ActionResult GetJob(Guid jobGuid)
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
        [HttpGet("category/{categoryId}")]
        public List<JobOffer> GetJobsByCategory(int categoryId)
        {
            CategoryInfo category = this.ScraperConfiguration.Categories.SingleOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return Array.Empty<JobOffer>().ToList();
            }

            List<JobOffer> jobs = this.DataContext.JobOffers
                .Where(j => !j.DateRemoved.HasValue)
                .OrderByDescending(j => j.DateLastChanged)
                .Where(j => j.Category.Equals(category.FriendlyName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return jobs;
        }

        // GET: api/jobs/filter
        [HttpGet("filter")]
        public List<JobOffer> GetJobsFiltered(
            [FromQuery] DateTime? changedAfter,
            [FromQuery] string contains,
            [FromQuery] decimal? minHourlyPay,
            [FromQuery] int? categoryId,
            [FromQuery, ModelBinder(typeof(QueryStringIntArrayModelBinder))] int[] categoryIds,
            [FromQuery] int? pageSize,
            [FromQuery] int? pageIndex,
            [FromQuery] bool excludeNonParsed = false)
        {
            IQueryable<JobOffer> jobs = this.DataContext.JobOffers
                .Where(j => !j.DateRemoved.HasValue)
                .OrderByDescending(j => j.DateLastChanged);

            if (changedAfter.HasValue)
            {
                jobs = jobs.Where(j => j.DateLastChanged > changedAfter);
            }

            if (!string.IsNullOrWhiteSpace(contains))
            {
                jobs = jobs.Where(j => j.Text.Contains(contains));
            }

            if (minHourlyPay.HasValue && minHourlyPay.Value > 0)
            {
                if (excludeNonParsed)
                {
                    jobs = jobs.Where(j =>
                        j.HourlyPay.HasValue &&
                        j.HourlyPay.Value >= minHourlyPay.Value);
                }
                else
                {
                    jobs = jobs.Where(j =>
                        !j.HourlyPay.HasValue ||
                        j.HourlyPay.Value >= minHourlyPay.Value);
                }
            }

            if (categoryIds != null && categoryIds.Length != 0)
            {
                string[] categoriesToFilterBy = this.ScraperConfiguration.Categories
                    .Where(c => categoryIds.Contains(c.Id))
                    .Select(c => c.FriendlyName)
                    .ToArray();

                jobs = jobs.Where(j => categoriesToFilterBy.Contains(j.Category));
            }
            else if (categoryId.HasValue)
            {
                CategoryInfo category = this.ScraperConfiguration.Categories.SingleOrDefault(c => c.Id == categoryId);

                if (category == null)
                {
                    return Array.Empty<JobOffer>().ToList();
                }

                jobs = jobs.Where(j => j.Category.Equals(category.FriendlyName, StringComparison.OrdinalIgnoreCase));
            }

            if(pageSize.HasValue && pageIndex.HasValue && pageSize > 0 && pageIndex >= 0)
            {
                jobs = jobs
                    .Skip(pageSize.Value * pageIndex.Value)
                    .Take(pageSize.Value);
            }

            return jobs.ToList();
        }

        // GET: api/jobs/code/{codeId}
        [HttpGet("code/{codeId}")]
        public List<JobOffer> GetJobsByCode(int codeId)
        {
            List<JobOffer> jobs = this.DataContext.JobOffers
                .Where(j => !j.DateRemoved.HasValue)
                .OrderByDescending(j => j.DateLastChanged)
                .Where(j => j.Code == codeId)
                .ToList();

            return jobs;
        }
    }
}
