using System;
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
        public ValuesController(StudentServisWebScraperDataContext context)
        {
            this.DataContext = context;
        }

        public StudentServisWebScraperDataContext DataContext { get; set; }

        // GET api/values
        [HttpGet]
        public List<JobOffer> Get()
        {
            List<JobOffer> offers = this.DataContext.JobOffers.ToList();

            return offers;
        }

        // GET api/values/5
        [HttpGet("{code}")]
        public JobOffer Get(int code)
        {
            JobOffer foundOffer = this.DataContext.JobOffers.SingleOrDefault(jo =>
                !jo.DateRemoved.HasValue &&
                jo.Code == code);

            return foundOffer;
        }

        // GET api/values/all
        [HttpGet("all")]
        public IEnumerable<JobOffer> GetAll([FromServices] StudentServisWebScraperDataContext ctx)
        {
            return ctx.JobOffers.ToList();
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
