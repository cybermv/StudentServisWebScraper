using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SSWS.Mobile.Data
{
    public class UserSettings
    {
        public const string ApplicationPropertiesKey = "SSWS.UserSettings";

        private UserSettings()
        {
        }

        public string ServiceUrl { get; set; }

        public DateTime LastRefreshDate { get; set; }

        public bool AllCategoriesSelected { get; set; }

        public List<int> SelectedCategories { get; set; }

        public decimal MinHourlyRate { get; set; }

        public bool ShowNonParsedJobs { get; set; }

        public bool ShowNotifications { get; set; }

        /// <summary>
        /// The default settings that will be used on first application start
        /// </summary>
        private static UserSettings GetInitialSettings()
        {
            return new UserSettings
            {
                ServiceUrl = @"http://ssws.azurewebsites.net/",
                LastRefreshDate = new DateTime(1993, 7, 6),
                AllCategoriesSelected = true,
                SelectedCategories = new List<int>(0),
                MinHourlyRate = 10, // LOL
                ShowNonParsedJobs = true,
                ShowNotifications = true
            };
        }

        public static UserSettings Load()
        {
            IDictionary<string, object> storage = Application.Current.Properties;

            if (!storage.ContainsKey(ApplicationPropertiesKey))
            {
                UserSettings firstSettings = GetInitialSettings();
                Store(firstSettings);
                return firstSettings;
            }

            string settingsJson = (string)storage[ApplicationPropertiesKey];
            UserSettings settings = JsonConvert.DeserializeObject<UserSettings>(settingsJson);

            return settings;
        }

        public static void Store(UserSettings settings)
        {
            IDictionary<string, object> storage = Application.Current.Properties;
            string settingsJson = JsonConvert.SerializeObject(settings);

            storage[ApplicationPropertiesKey] = settingsJson;
        }
    }
}
