using System.Threading.Tasks;
using SSWS.Mobile.Data.Interfaces;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Text;
using SSWS.Mobile.Data;

[assembly: Xamarin.Forms.Dependency(typeof(HttpClientUserSettingsStore))]
namespace SSWS.Mobile.Data
{
    public class HttpClientUserSettingsStore : IUserSettingsStore
    {
        public async Task<UserSettings> LoadSettings(string id)
        {
            string response = "{}";

            try
            {
                HttpClient client = HttpClientProvider.GetClient();
                Uri requestUri = new Uri("/api/usersettings/" + id);
                response = await client.GetStringAsync(requestUri);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }

            UserSettings settings = JsonConvert.DeserializeObject<UserSettings>(response);

            return settings;
        }

        public async Task<bool> SaveSettings(string id, UserSettings settings)
        {
            string payload = JsonConvert.SerializeObject(settings);

            try
            {
                HttpClient client = HttpClientProvider.GetClient();
                Uri requestUri = new Uri("/api/usersettings/" + id);
                HttpResponseMessage response = await client
                    .PostAsync(requestUri, new StringContent(payload, Encoding.UTF8, "application/json"));

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                return false;
            }
        }

        public async Task<bool> ClearSettings(string id)
        {
            try
            {
                HttpClient client = HttpClientProvider.GetClient();
                Uri requestUri = new Uri("/api/usersettings/" + id);
                HttpResponseMessage response = await client.DeleteAsync(requestUri);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                return false;
            }
        }
    }
}
