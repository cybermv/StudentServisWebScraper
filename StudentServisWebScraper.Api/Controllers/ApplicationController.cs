using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Models;
using StudentServisWebScraper.Api.Scraping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentServisWebScraper.Api.Controllers
{
    [Route("[controller]")]
    public class ApplicationController : Controller
    {
        public ApplicationController(ScraperConfiguration configuration, StudentServisWebScraperDataContext context, ILogger<ApplicationController> logger)
        {
            this.ScraperConfiguration = configuration;
            this.DataContext = context;
            this.Logger = logger;
        }

        public ScraperConfiguration ScraperConfiguration { get; set; }

        public StudentServisWebScraperDataContext DataContext { get; set; }

        public ILogger<ApplicationController> Logger { get; set; }

        // GET: /
        // GET: /Application
        [HttpGet("/"), HttpGet("/Application")]
        public IActionResult IndexEmpty()
        {
            return RedirectToActionPermanent(nameof(ApplicationController.Index));
        }

        // GET: Application/Index
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Application/Jobs
        [HttpGet("Jobs")]
        public IActionResult Jobs()
        {
            return View();
        }

        // GET: Application/Documentation
        [HttpGet("Documentation")]
        public IActionResult Documentation()
        {
            return View();
        }

        // GET: Application/Administration
        [HttpGet("Administration"), Authorize]
        public IActionResult Administration()
        {
            List<JobOffer> allOffers = this.DataContext.JobOffers.ToList();

            AdministrationViewModel model = new AdministrationViewModel
            {
                TotalActiveCount = allOffers.Count(j => !j.DateRemoved.HasValue),
                TotalDeletedCount = allOffers.Count(j => j.DateRemoved.HasValue),
                TotalUnparsedCount = allOffers.Count(j => !j.HourlyPay.HasValue),
                AverageNewJobsPerDay = allOffers
                    .GroupBy(j => j.DateAdded.Date, j => j)
                    .Select(g => new KeyValuePair<DateTime, int>(g.Key, g.Count()))
                    .Average(kv => kv.Value),
                AverageDeletedJobsPerDay = allOffers
                    .Where(j => j.DateRemoved.HasValue)
                    .GroupBy(j => j.DateRemoved.Value.Date, j => j)
                    .Select(g => new KeyValuePair<DateTime, int>(g.Key, g.Count()))
                    .Average(kv => kv.Value),
                AverageHourlyPay = allOffers
                    .Where(j => j.HourlyPay.HasValue)
                    .Average(j => j.HourlyPay.Value),
                AverageJobParsingSuccesses = 1 - 
                    ((double)allOffers.Count(j => !j.HourlyPay.HasValue) / (double)allOffers.Count),
                ByCategoryStatistics = allOffers
                    .GroupBy(j => j.Category)
                    .Select(g => new JobByCategoryStatistics
                    {
                        Category = g.Key,
                        ActiveCount = g.Count(j => !j.DateRemoved.HasValue),
                        DeletedCount = g.Count(j => j.DateRemoved.HasValue),
                        AverageHourlyPay = g
                            .Where(j => j.HourlyPay.HasValue)
                            .Average(j => j.HourlyPay.Value)
                    })
                    .ToList()
            };

            return View(model);
        }

        // GET: Application/Login
        [HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Application/Login
        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            LoginViewModel model,
            [FromServices] SignInManager<IdentityUser> signInManager,
            string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(
                    model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    Logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Krivo korisničko ime ili lozinka.");
                    return View(model);
                }
            }

            return View(model);
        }

        // POST: Application/Logout
        [HttpPost("Logout"), Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout([FromServices] SignInManager<IdentityUser> signInManager)
        {
            await signInManager.SignOutAsync();
            Logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Application");
        }

        #region Helpers

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Application");
            }
        }

        #endregion Helpers
    }
}
