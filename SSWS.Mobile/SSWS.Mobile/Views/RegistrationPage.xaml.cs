using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegistrationPage : ContentPage
	{
        private ListView _jobOffersList;

        private Entry _usernameTbx;
        private Entry _phoneNumberTbx;
        private Button _confirmBtn;

        public RegistrationPage(ListView jobOffersList)
		{
            _jobOffersList = jobOffersList;
			InitializeComponent();
            BuildLayout();
		}

        private void BuildLayout()
        {
            StackLayout stack = new StackLayout
            {
                Margin = new Thickness(20, 50)
            };

            stack.Children.Add(new Label
            {
                Text = Localise("StrRegisterTitle"),
                FontSize = 24,
                HorizontalTextAlignment = TextAlignment.Center
            });

            stack.Children.Add(new Label
            {
                Text = Localise("StrRegisterText"),
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Center
            });

            _usernameTbx = new Entry { Placeholder = "Ivan Horvat", Keyboard = Keyboard.Text };
            _usernameTbx.TextChanged += Entry_TextChanged;
            stack.Children.Add(_usernameTbx);

            _phoneNumberTbx = new Entry { Placeholder = "090 1234 567", Keyboard = Keyboard.Telephone };
            _phoneNumberTbx.TextChanged += Entry_TextChanged;
            stack.Children.Add(_phoneNumberTbx);

            _confirmBtn = new Button();
            _confirmBtn.Clicked += ConfirmBtn_Clicked;
            stack.Children.Add(_confirmBtn);

            this.Content = stack;
        }

        protected override void OnAppearing()
        {
            Entry_TextChanged(null, null);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid =
                !string.IsNullOrWhiteSpace(_usernameTbx.Text) &&
                !string.IsNullOrWhiteSpace(_phoneNumberTbx.Text) &&
                _usernameTbx.Text.Length >= 5 &&
                _phoneNumberTbx.Text.Length >= 8 &&
                _phoneNumberTbx.Text.All(char.IsDigit);

            _confirmBtn.IsEnabled = isValid;
            _confirmBtn.Text = Localise(isValid ? "StrRegisterConfirmText" : "StrRegisterConfirmTextError");
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            IUserIdProvider idProvider = DependencyService.Get<IUserIdProvider>();
            IUserSettingsStore settingsStore = DependencyService.Get<IUserSettingsStore>();

            string newId = $"SSWS-{Guid.NewGuid()}";
            idProvider.Set(newId);

            UserSettings settings = await settingsStore.LoadSettings(newId);
            settings.Username = _usernameTbx.Text;
            settings.PhoneNumber = _phoneNumberTbx.Text;
            await settingsStore.SaveSettings(newId, settings);

            _jobOffersList.BeginRefresh();
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        private string Localise(string key)
        {
            object result = "NF: " + key;
            Application.Current.Resources.TryGetValue(key, out result);
            return result.ToString();
        }
    }
}