using Xamarin.Forms;
using SSWS.Mobile.Views;
using Matcha.BackgroundService;
using SSWS.Mobile.Tasks;

namespace SSWS.Mobile
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                MainPage = new JobOffers();
            }
            else
            {
                MainPage = new NavigationPage(new JobOffers());
            }
		}

		protected override void OnStart()
		{
            // Handle when your app starts
            BackgroundAggregatorService.Add(() => new CheckJobsTask());

            BackgroundAggregatorService.StartBackgroundService();
        }

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
            // Handle when your app resumes
        }
	}
}