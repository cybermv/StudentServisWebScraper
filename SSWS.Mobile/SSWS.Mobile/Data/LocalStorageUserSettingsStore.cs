﻿using Newtonsoft.Json;
using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageUserSettingsStore))]
namespace SSWS.Mobile.Data
{
    public class LocalStorageUserSettingsStore : IUserSettingsStore
    {
        public const string ApplicationPropertiesKey = "SSWS.UserSettings";

        /// <summary>
        /// The default settings that will be used on first application start
        /// </summary>
        private static UserSettings GetInitialSettings()
        {
            return new UserSettings
            {
                LastRefreshDate = new DateTime(1993, 7, 6),
                AllCategoriesSelected = true,
                SelectedCategories = new List<int>(0),
                MinHourlyRate = 10, // LOL
                ShowNonParsedJobs = true,
                ShowNotifications = true
            };
        }

        public UserSettings LoadSettings(string _)
        {
            IDictionary<string, object> storage = Application.Current.Properties;

            if (!storage.ContainsKey(ApplicationPropertiesKey))
            {
                UserSettings firstSettings = GetInitialSettings();
                SaveSettings(_, firstSettings);
                return firstSettings;
            }

            string settingsJson = (string)storage[ApplicationPropertiesKey];
            UserSettings settings = JsonConvert.DeserializeObject<UserSettings>(settingsJson);

            return settings;
        }

        public bool SaveSettings(string _, UserSettings settings)
        {
            IDictionary<string, object> storage = Application.Current.Properties;
            string settingsJson = JsonConvert.SerializeObject(settings);

            storage[ApplicationPropertiesKey] = settingsJson;
            return true;
        }
    }
}
