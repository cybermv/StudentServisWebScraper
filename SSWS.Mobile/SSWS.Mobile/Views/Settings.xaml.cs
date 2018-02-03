using SSWS.Mobile.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Settings : ContentPage
	{
        public UserSettings UserSettings { get; set; }

        public Settings()
		{
			InitializeComponent();

            UserSettings = UserSettings.Load();
            this.BindingContext = this;
		}
	}
}