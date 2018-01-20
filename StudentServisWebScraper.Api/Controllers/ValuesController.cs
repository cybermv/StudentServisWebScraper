﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StudentServisWebScraper.Api.Scraping;
using StudentServisWebScraper.Api.Scraping.Models;
using StudentServisWebScraper.Api.Data;

namespace StudentServisWebScraper.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        public ScraperConfiguration ScraperConfiguration { get; }

        public JobScraper Scraper { get; }

        public JobOfferParser Parser { get; }

        public ValuesController(ScraperConfiguration config, JobScraper scraper, JobOfferParser parser)
        {
            this.ScraperConfiguration = config;
            this.Scraper = scraper;
            this.Parser = parser;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<JobOffer> Get()
        {
            ICollection<JobOfferInfo> scrapedJobs = this.Scraper.Scrape();

            IEnumerable<JobOffer> parsedJobs = this.Parser.Parse(scrapedJobs);

            return parsedJobs;

            //return new string[] { "value1", "value2", ScraperConfiguration.RootUrl.ToString(), ScraperConfiguration.JobOfferUrl.ToString() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
