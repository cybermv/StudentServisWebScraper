using SSWS.Mobile.Data;
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
            
            await Navigation.PushAsync(new JobOfferDetails(tappedModel));

            ((ListView)sender).SelectedItem = null;
        }

        private async void JobOffersListView_Refreshing(object sender, EventArgs e)
        {
            await LoadJobOffers();
            ((ListView)sender).EndRefresh();
        }

        private async Task Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async Task LoadJobOffers()
        {
            IJobOfferRepository jobsRepo = DependencyService.Get<IJobOfferRepository>();
            UserSettings settings = UserSettings.Load();

            // load all jobs using these filters:
            // - by selected categories (null or empty array = no filter)
            // - by the minimum hourly rate
            List<JobModel> loadedJobs = await jobsRepo.GetJobOffers(
                categoryIds: settings.SelectedCategories.ToArray(),
                minHourlyPay: settings.MinHourlyRate);

            settings.LastRefreshDate = DateTime.Now;
            UserSettings.Store(settings);

            JobOffersListView.ItemsSource = new ObservableCollection<JobModel>(loadedJobs);
        }
    }
}
