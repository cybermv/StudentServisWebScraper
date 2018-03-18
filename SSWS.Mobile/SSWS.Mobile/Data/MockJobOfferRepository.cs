using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SSWS.Mobile.Models;
using System.Linq;

namespace SSWS.Mobile.Data
{
    public class MockJobOfferRepository : IJobOfferRepository
    {
        public Task<List<JobModel>> GetJobOffers(DateTime? changedAfter = null, int[] categoryIds = null, decimal? minHourlyPay = null, bool excludeNonParsed = false)
        {
            List<JobModel> jobs = new List<JobModel>();

            Random r = new Random();
            string[] categories = { "M Rad u skladištima", "M Poslovi u ugostiteljstvu", "M Poslovi u proizvodnji", "M Razni poslovi" };
            string[] texts =
            {
                "/ Promocija HomeOgarden proizvoda. Traži se komunikativna osoba, koja sama prilazi ljudima, poželjna znanja o vrtlarenju, prednost studenti/ice agronomije. Radno vrijeme: petak, subota, nedjelja od 15.02.-15.05.2018. Naknada po satu 25kn. Prijave na: lea.rebrovic@homeogarden.com",
                "/ KANAAN d.o.o. traži studente/ice za promociju premium prozvoda na promo-štandu u poslovnicama Konzuma - Radnička 49; Črnomerec; Huzjanova 2. Radno vrijeme: 3 vikenda od 26.01.-12.02.2018., petak 16-20 kn, subota i nedjelja 09-13 h. Naknada po satu: 20,00 kn. Kontakt: 099/528-4616; matej.srb@kanaan.hr",
                "/ Brand Buzz traži studenate/ice za rad na promocijama/degustacijama u velikim trgovačkim centrima. Radno vrijeme: petak 16-20 h i subota 10-14 h. Potrebna komunikativnost, pouzdanost  i  volja za radom. Satnica: 25 kn. Ukoliko se prepoznaješ u traženim osobinama, pošalji nam kratki CV i pridruži se našem timu! Prijave na: promocije.hr1@gmail.com",
                "/ Candy Hoover Zagreb d.o.o. traži studente/ice za poslove promocije proizvoda brandova Candy i Hoover po Zagrebu. Traže se komunikativne, vedre, prilagodljive ali prije svega odgovorne i pouzdane osobe. Radno vrijeme: od petka do nedjelje u poslijepodnevnim satima. Naknada po satu: 25,00 kn. Životopise slati do 15.02.2018. na: ana.slisko@candy.hr",
                "/ Prodajni savjetnik/ica u call centru (MODERNE KOMUNIKACIJE D.O.O.). Potrebne razvijene komunikacijske i prezentacijske vještine, odgovorno i savjesno obavljanje radnih zadataka, upornost i predanost u postizanju zadanih ciljeva, velika samostalnost i ujedno spremnost na timski rad. Radno vrijeme: Fleksibilno, minimalno 20 sati tjedno. Na duži period. Naknada po satu:20,00 kn + bonusi. Prijave na e-mail: posao@modernekomunikacije.hr",
                "/ Telefonski komercijalist/ica - tražimo komunikativnu i elokventnu osobu za telefonsko kontaktiranje potencijalnih korisnika obnovljivih izvora energije, vađenje novih kontakata (email adrese i kontakt telefoni) i vođenje Excel tablice o rezultatima rada. Traži se napredno poznavanje Office paketa (naročito Excel) i služenje internetom. Kruge 48; mogućnost rada od kuće. Radno vrijeme: 9-17h. Period rada: dva mjeseca. Naknada po satu: 20-25 kn/h s mogućnošću povišenja satnice. Ostalo: poznavanje sustava obnovljivih izvora energije je prednost. Kontakt: 01 6600 434, 099 702 4279, administracija@itrs.hr",
                "/ Tražimo komunikativne studente/ice koji će zaprimati dolazne pozive i odgovarati na upite kupaca. Satnica iznosi 25,00 Kn. Posao se odvija u uredu u sjedištu tvrtke, u blizini Arena centra. Radno vrijeme je 8 sati dnevno (ponedjeljak – petak od 08:00 do 14:00). Poslati životopis na: kadrovska.sluzba@mozaik-knjiga.hr ili 01/6315-111. Rok prijave: 31.01.2018."
            };

            decimal?[] satnice;

            if (excludeNonParsed)
            {
                satnice = new decimal?[] { 15, 18, 20, 28, 30, 45 };
            }
            else
            {
                satnice = new decimal?[] { 15, 18, 20, 28, 30, null };
            }

            

            for (int i = 0; i < r.Next(50, 250); i++)
            {
                jobs.Add(new JobModel
                {
                    Code = i + 1,
                    Category = categories[r.Next(0, categories.Length)],
                    HourlyPay = satnice[r.Next(0, satnice.Length)],
                    DateAdded = DateTime.Now,
                    DateLastChanged = DateTime.Now,
                    Text = (i + 1) + texts[r.Next(0, texts.Length)],
                    ContactEmail = r.Next(0, 10) > 7 ? "contact@gmail.com" : null,
                    ContactPhone = r.Next(0, 10) > 5 ? "01/232-2518" : null,
                });
            }
            
            return Task.FromResult(jobs);
        }

        public Task<List<CategoryModel>> GetCategories()
        {
            string[] categories = { "M Rad u skladištima", "M Poslovi u ugostiteljstvu", "M Poslovi u proizvodnji", "M Razni poslovi" };

            return Task.FromResult(categories
                .Select((c, idx) => new CategoryModel { Id = idx, FriendlyName = c, ScrapeName = c })
                .ToList());
        }
    }
}
