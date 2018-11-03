using System;
using System.Threading.Tasks;
using Matcha.BackgroundService;
using Xamarin.Forms;
using SSWS.Mobile.Notifications;
using SSWS.Mobile.Data.Interfaces;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using System.Collections.Generic;

namespace SSWS.Mobile.Tasks
{
    public class CheckJobsTask : IPeriodicTask
    {
        private IUserIdProvider idProvider;
        private IUserSettingsStore settingsStore;
        private IJobOfferRepository repository;
        private IJobNotificator notificator;

        public CheckJobsTask()
        {
            this.Interval = TimeSpan.FromMinutes(2); // TODO: move to config?
            this.idProvider = DependencyService.Get<IUserIdProvider>();
            this.settingsStore = DependencyService.Get<IUserSettingsStore>();
            this.repository = DependencyService.Get<IJobOfferRepository>();
            this.notificator = DependencyService.Get<IJobNotificator>();
        }

        public TimeSpan Interval { get; set; }

        public async Task StartJob()
        {
            // TODO avoid unneccessary call to server
            string userId = idProvider.Get();
            UserSettings settings = await settingsStore.LoadSettings(userId);

            if (!settings.ShowNotifications) { return; }

            List<JobModel> foundJobs = await repository.GetJobOffers(
                categoryIds: settings.SelectedCategories.ToArray(),
                minHourlyPay: settings.MinHourlyRate,
                excludeNonParsed: !settings.ShowNonParsedJobs,
                changedAfter: settings.LastRefreshDate);

            if (foundJobs.Count > 0)
            {
                notificator.Notify(foundJobs.Count);
            }
        }
    }
}
