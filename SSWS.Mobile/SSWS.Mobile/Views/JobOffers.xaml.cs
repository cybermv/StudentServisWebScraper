using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobOffers : ContentPage
    {
        public JobOffers()
        {
            InitializeComponent();
            JobOffersListView.BeginRefresh();
        }

        private async void JobOffersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            JobModel tappedModel = null;
            if ((tappedModel = e.Item as JobModel) == null)
            {
                return;
            }

            ((ListView)sender).SelectedItem = null;
            await Navigation.PushAsync(new JobOfferDetails(tappedModel));
        }

        private async void JobOffersListView_Refreshing(object sender, EventArgs e)
        {
            await LoadJobOffers();
            ((ListView)sender).EndRefresh();
        }

        private async Task Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage(this.JobOffersListView));
        }

        private async Task LoadJobOffers()
        {
            IJobOfferRepository jobsRepo = DependencyService.Get<IJobOfferRepository>();
            IUserSettingsStore settingsStore = DependencyService.Get<IUserSettingsStore>();
            IUserIdProvider idProvider = DependencyService.Get<IUserIdProvider>();

            if (!idProvider.Exists())
            {
                // user is not registered
                RegistrationPage registration = new RegistrationPage(this.JobOffersListView);
                await Navigation.PushModalAsync(registration);
                return;
            }

            string id = idProvider.Get();
            UserSettings settings = await settingsStore.LoadSettings(id);

            // load all jobs using these filters:
            // - by selected categories (null or empty array = no filter)
            // - by the minimum hourly rate
            // - show or hide non-parsed jobs
            List<JobModel> loadedJobs = await jobsRepo.GetJobOffers(
                categoryIds: settings.SelectedCategories.ToArray(),
                minHourlyPay: settings.MinHourlyRate,
                excludeNonParsed: !settings.ShowNonParsedJobs);

            settings.LastRefreshDate = DateTime.UtcNow;
            await settingsStore.SaveSettings(id, settings);

            JobOffersListView.ItemsSource = new ObservableCollection<JobModel>(loadedJobs);
        }
    }
}
