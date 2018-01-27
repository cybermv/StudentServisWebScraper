using Newtonsoft.Json;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobOffers : ContentPage
    {
        public ObservableCollection<JobModel> JobOffersCollection { get; set; }

        public JobOffers()
        {
            InitializeComponent();

            JobOffersCollection = new ObservableCollection<JobModel>();
            JobOffersListView.ItemsSource = JobOffersCollection;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await LoadJobOffers();
        }

        private async Task LoadJobOffers()
        {
            var repo = DependencyService.Get<IJobOfferRepository>();

            List<JobModel> jobs = await repo.GetJobOffers(new DateTime(2015, 1, 1), new int[] { 1, 2, 3, 7 }, 28);

            JobOffersCollection.Clear();

            foreach (var job in jobs)
            {
                JobOffersCollection.Add(job);
            }
        }

        private async void JobOffersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            JobModel tappedModel = null;
            if ((tappedModel = e.Item as JobModel) == null)
            {
                return;
            }

            //await DisplayAlert("Job tapped", $"A job was tapped, code: {tappedModel.Code}", "OK");

            await Navigation.PushAsync(new JobOfferDetails(tappedModel));

            ((ListView)sender).SelectedItem = null;
        }

        private async void JobOffersListView_Refreshing(object sender, EventArgs e)
        {
            await LoadJobOffers();
            ((ListView)sender).EndRefresh();
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {

        }
    }
}
