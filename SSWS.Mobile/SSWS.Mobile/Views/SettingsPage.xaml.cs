using SSWS.Mobile.Controls;
using SSWS.Mobile.Data;
using SSWS.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSWS.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private UserSettings _currentSettings;

        private TableSection _categoriesSection;
        private TableSection _generalSection;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            _currentSettings = UserSettings.Load();
            this.Content = await SetUpTableLayout();
        }

        protected override void OnDisappearing()
        {
            // TODO: save
            base.OnDisappearing();
        }

        private async Task<TableView> SetUpTableLayout()
        {
            // categories
            _categoriesSection = new TableSection(Localise("StrCategoriesSelect"));
            IJobOfferRepository jobOfferRepository = DependencyService.Get<IJobOfferRepository>();
            List<CategoryModel> categories = await jobOfferRepository.GetCategories();

            SwitchCell allSelect = new TaggableSwitchCell<int>
            {
                Text = Localise("StrCategoriesAll"),
                On = _currentSettings.AllCategoriesSelected || _currentSettings.SelectedCategories.Count == 0,
                Tag = -1
            };
            allSelect.OnChanged += AllSelect_OnChanged;
            _categoriesSection.Add(allSelect);

            foreach (CategoryModel category in categories)
            {
                SwitchCell categoryCell = new TaggableSwitchCell<int>
                {
                    Text = category.FriendlyName,
                    On = _currentSettings.SelectedCategories.Contains(category.Id),
                    Tag = category.Id
                };
                categoryCell.OnChanged += Category_OnChanged;
                _categoriesSection.Add(categoryCell);
            }

            // general section
            _generalSection = new TableSection(Localise("StrGeneralSettings"))
            {
                new TaggableEntryCell<string>
                {
                    Label = Localise("StrGeneralMinPay"),
                    Keyboard = Keyboard.Numeric,
                    Text = _currentSettings.MinHourlyRate.ToString(),
                    HorizontalTextAlignment = TextAlignment.End,
                    Tag = nameof(_currentSettings.MinHourlyRate)
                },
                new TaggableSwitchCell<string>
                {
                    Text = Localise("StrGeneralNonParsed"),
                    On = _currentSettings.ShowNonParsedJobs,
                    Tag = nameof(_currentSettings.ShowNonParsedJobs)
                },
                new TaggableSwitchCell<string>
                {
                    Text = Localise("StrGeneralNotifications"),
                    On = _currentSettings.ShowNotifications,
                    Tag = nameof(_currentSettings.ShowNotifications)
                }
            };

            TableSection aboutSection = new TableSection(Localise("StrAboutSection"))
            {
                new ViewCell
                {
                    View = new Label
                    {
                        Text = Localise("StrAboutSectionText"),
                        Margin = 10
                    },
                }
            };

            return new TableView
            {
                Root = new TableRoot(Localise("StrSettings"))
                {
                    _generalSection,
                    _categoriesSection,
                    aboutSection
                },
                Intent = TableIntent.Settings,
                HasUnevenRows = true
            };
        }

        private void CollectChanges()
        {
            // categories
            _currentSettings.AllCategoriesSelected = _categoriesSection
                .OfType<TaggableSwitchCell<int>>()
                .Single(c => c.Tag == -1).On;

            if (!_currentSettings.AllCategoriesSelected)
            {
                _currentSettings.SelectedCategories = _categoriesSection
                    .OfType<TaggableSwitchCell<int>>()
                    .Where(c => c.Tag >= 0 && c.On)
                    .Select(c => c.Tag)
                    .ToList();
            }
            else
            {
                _currentSettings.SelectedCategories.Clear();
            }

            // general
            string amountText = _generalSection
                .OfType<TaggableEntryCell<string>>()
                .Single(c => c.Tag == nameof(_currentSettings.MinHourlyRate)).Text;
            decimal amountParsed = _currentSettings.MinHourlyRate;
            decimal.TryParse(amountText, out amountParsed);
            _currentSettings.MinHourlyRate = amountParsed;

            _currentSettings.ShowNotifications = _generalSection
                .OfType<TaggableSwitchCell<string>>()
                .Single(c => c.Tag == nameof(_currentSettings.ShowNotifications)).On;

            _currentSettings.ShowNonParsedJobs = _generalSection
                .OfType<TaggableSwitchCell<string>>()
                .Single(c => c.Tag == nameof(_currentSettings.ShowNonParsedJobs)).On;


        }

        private void AllSelect_OnChanged(object sender, ToggledEventArgs e)
        {
            SwitchCell allSelectCell = (SwitchCell)sender;
            if (allSelectCell.On)
            {
                foreach (SwitchCell cell in _categoriesSection)
                {
                    if (cell == allSelectCell) continue;
                    cell.On = false;
                }
            }
        }

        private void Category_OnChanged(object sender, ToggledEventArgs e)
        {
            SwitchCell currentCell = (SwitchCell)sender;
            if (currentCell.On == true)
            {
                SwitchCell allSelectCell = (SwitchCell)_categoriesSection.First();
                allSelectCell.On = false;
            }
        }

        private async void SaveSettings_Clicked(object sender, EventArgs e)
        {
            CollectChanges();
            UserSettings.Store(_currentSettings);
            await Navigation.PopToRootAsync();
        }

        private string Localise(string key)
        {
            object result = "NF: " + key;
            Application.Current.Resources.TryGetValue(key, out result);
            return result.ToString();
        }
    }
}