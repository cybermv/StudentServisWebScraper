using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StudentServisWebScraper.Api.Data;
using StudentServisWebScraper.Api.Models;
using Newtonsoft.Json;
using System.Net;

namespace StudentServisWebScraper.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserSettingsController : Controller
    {
        public UserSettingsController(StudentServisWebScraperDataContext context)
        {
            this.DataContext = context;
        }

        public StudentServisWebScraperDataContext DataContext { get; set; }

        // GET: api/usersettings/{id}
        [HttpGet("{id}")]
        public ObjectResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, null);
            }

            UserSettings settings = this.DataContext.UserSettings
                .SingleOrDefault(s => string.Equals(s.UserIdentifier, id, StringComparison.OrdinalIgnoreCase));

            if (settings != null)
            {
                UserSettingsViewModel model = JsonConvert.DeserializeObject<UserSettingsViewModel>(settings.SettingsJson);
                return StatusCode((int)HttpStatusCode.OK, model);
            }
            else
            {
                UserSettingsViewModel initialSettings = new UserSettingsViewModel
                {
                    LastRefreshDate = new DateTime(1993, 7, 6),
                    AllCategoriesSelected = true,
                    SelectedCategories = new List<int>(0),
                    MinHourlyRate = 10, // LOL
                    ShowNonParsedJobs = true,
                    ShowNotifications = true
                };
                return StatusCode((int)HttpStatusCode.OK, initialSettings);
            }
        }

        // POST: api/usersettings/{id}
        [HttpPost("{id}")]
        public StatusCodeResult Post([FromRoute] string id, [FromBody] UserSettingsViewModel model)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            UserSettings settings = this.DataContext.UserSettings
                .SingleOrDefault(s => s.UserIdentifier == id);

            if (settings == null)
            {
                settings = new UserSettings
                {
                    UserIdentifier = id,
                    SettingsJson = JsonConvert.SerializeObject(model)
                };
                this.DataContext.UserSettings.Add(settings);
            }
            else
            {
                settings.SettingsJson = JsonConvert.SerializeObject(model);
                this.DataContext.UserSettings.Update(settings);
            }

            bool success = this.DataContext.SaveChanges() > 0;

            return StatusCode((int)(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError));
        }

        // DELETE: api/usersettings/{id}
        [HttpDelete("{id}")]
        public StatusCodeResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }

            UserSettings settings = this.DataContext.UserSettings
                .SingleOrDefault(s => string.Equals(s.UserIdentifier, id, StringComparison.OrdinalIgnoreCase));

            if (settings == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }

            this.DataContext.UserSettings.Remove(settings);
            bool success = this.DataContext.SaveChanges() > 0;

            return StatusCode((int)(success ? HttpStatusCode.OK : HttpStatusCode.InternalServerError));
        }
    }
}