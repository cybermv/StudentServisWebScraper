using System;
using System.Collections.Generic;

namespace SSWS.Mobile.Data
{
    public class UserSettings
    {
        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime LastRefreshDate { get; set; }

        public bool AllCategoriesSelected { get; set; }

        public List<int> SelectedCategories { get; set; }

        public decimal MinHourlyRate { get; set; }

        public bool ShowNonParsedJobs { get; set; }

        public bool ShowNotifications { get; set; }
    }
}
