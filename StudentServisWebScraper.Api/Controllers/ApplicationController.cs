using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Models;
using StudentServisWebScraper.Api.Scraping;
using System.Threading.Tasks;

namespace StudentServisWebScraper.Api.Controllers
{
    [Route("[controller]"), Authorize]
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
        [HttpGet("/"), HttpGet("/Application"), AllowAnonymous]
        public IActionResult IndexEmpty()
        {
            return RedirectToActionPermanent(nameof(ApplicationController.Index));
        }

        // GET: Application/Index
        [HttpGet("Index"), AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        // GET: Application/Documentation
        [HttpGet("Documentation"), AllowAnonymous]
        public IActionResult Documentation()
        {
            return View();
        }

        // GET: Application/Login
        [HttpGet("Login"), AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Application/Login
        [HttpPost("Login"), AllowAnonymous, ValidateAntiForgeryToken]
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
        [HttpPost("Logout"), ValidateAntiForgeryToken]
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
