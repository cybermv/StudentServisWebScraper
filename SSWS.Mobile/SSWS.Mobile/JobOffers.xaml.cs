using Newtonsoft.Json;
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

namespace SSWS.Mobile
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
            Random r = new Random();
            string[] categories = { "Fizicki", "Ugostiteljstvo", "Trgovina" };
            decimal?[] satnice = { 15, 20, 28, null };

            JobOffersCollection.Clear();

            for (int i = 0; i < r.Next(50, 250); i++)
            {
                JobOffersCollection.Add(new JobModel
                {
                    Code = r.Next(1, 150),
                    Category = categories[r.Next(0, categories.Length)],
                    HourlyPay = satnice[r.Next(0, satnice.Length)],
                    DateAdded = DateTime.Now,
                    Text = "Ovo je neki sample text oglasa."
                });
            }

            /*string resp = "[{'id':'8c4c8cca-169e-475a-7225-08d564cf5009','text':'216/ Galeb dalmatinska trikotaža d.d  traži studenta/icu za svakodnevno dijeljenje letaka u Avenue Mallu. Radno vrijeme: od 16-20 h. Cijena sata: 25,00 kn. Prijave na: tina.gotovac@galeb.hr','code':216,'category':'Poslovi promidžbe','dateAdded':'2018-01-26T15:13:00.514867','dateLastChanged':null,'dateRemoved':null,'contactPhone':null,'contactEmail':'tina.gotovac@galeb.hr','hourlyPay':null},{'id':'eecc0e32-c54d-4f5a-7226-08d564cf5009','text':'219/ Portal HRVATSKI POSLOVNI IMENIK traži telefonske operatere - prodajne agente/ice za prezentaciju i prodaju marketinških usluga tvrtke pravnim osobama (internet oglašavanja, Google oglašavanja, marketinga na društvenim mrežama …). Potrebne dobro razvijene komunikacijske vještine, jasno i razgovijetno izražavanje, interes za područje telefonske prodaje, Črnomerec. Radno vrijeme: 9.00 – 16.00. Period rada: 90 i više dana. Naknada po satu: 20 kn= od 3.000,00 kn + BONUS prema ostvarenim rezultatima. Vaše prijave sa životopisom očekujemo do 28.02.2018. na e-mail: marketing@hrvatski-poslovni-imenik.com s naznakom „SC Prijava za radno mjesto – Prodajni agent“','code':219,'category':'Poslovi promidžbe','dateAdded':'2018-01-26T15:13:00.514867','dateLastChanged':null,'dateRemoved':null,'contactPhone':null,'contactEmail':'marketing@hrvatski-poslovni-imenik.com','hourlyPay':20.00},{'id':'15a02ec1-78d6-4c43-7227-08d564cf5009','text':'220/ Promocija HomeOgarden proizvoda. Traži se komunikativna osoba, koja sama prilazi ljudima, poželjna znanja o vrtlarenju, prednost studenti/ice agronomije. Radno vrijeme: petak, subota, nedjelja od 15.02.-15.05.2018. Naknada po satu 25kn. Prijave na: lea.rebrovic@homeogarden.com','code':220,'category':'Poslovi promidžbe','dateAdded':'2018-01-26T15:13:00.514867','dateLastChanged':null,'dateRemoved':null,'contactPhone':null,'contactEmail':'lea.rebrovic@homeogarden.com','hourlyPay':null},{'id':'55da4ca0-9a27-4f71-7228-08d564cf5009','text':'236/ KANAAN d.o.o. traži studente/ice za promociju premium prozvoda na promo-štandu u poslovnicama Konzuma - Radnička 49; Črnomerec; Huzjanova 2. Radno vrijeme: 3 vikenda od 26.01.-12.02.2018., petak 16-20 kn, subota i nedjelja 09-13 h. Naknada po satu: 20,00 kn. Kontakt: 099/528-4616; matej.srb@kanaan.hr','code':236,'category':'Poslovi promidžbe','dateAdded':'2018-01-26T15:13:00.514867','dateLastChanged':null,'dateRemoved':null,'contactPhone':'099/528-4616','contactEmail':'matej.srb@kanaan.hr','hourlyPay':20.00},{'id':'7a8cb44e-bbbc-437f-7229-08d564cf5009','text':'244/ Brand Buzz traži studenate/ice za rad na promocijama/degustacijama u velikim trgovačkim centrima. Radno vrijeme: petak 16-20 h i subota 10-14 h. Potrebna komunikativnost, pouzdanost  i  volja za radom. Satnica: 25 kn. Ukoliko se prepoznaješ u traženim osobinama, pošalji nam kratki CV i pridruži se našem timu! Prijave na: promocije.hr1@gmail.com','code':244,'category':'Poslovi promidžbe','dateAdded':'2018-01-26T15:13:00.514867','dateLastChanged':null,'dateRemoved':null,'contactPhone':null,'contactEmail':'promocije.hr1@gmail.com','hourlyPay':25.00}]";

            List<JobModel> jobs = JsonConvert.DeserializeObject<List<JobModel>>(resp);
            JobOffersCollection = new ObservableCollection<JobModel>(jobs);*/
            
        }

        private async void JobOffersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            JobModel tappedModel = null;
            if ((tappedModel = e.Item as JobModel) == null)
            {
                return;
            }

            await DisplayAlert("Job tapped", $"A job was tapped, code: {tappedModel.Code}", "OK");

            ((ListView)sender).SelectedItem = null;
        }

        private async void JobOffersListView_Refreshing(object sender, EventArgs e)
        {
            await LoadJobOffers();
            ((ListView)sender).EndRefresh();
        }
    }
}
