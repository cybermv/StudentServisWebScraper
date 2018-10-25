using System.Threading.Tasks;

namespace SSWS.Mobile.Data.Interfaces
{
    public interface IUserSettingsStore
    {
        Task<UserSettings> LoadSettings(string id);
        Task<bool> SaveSettings(string id, UserSettings settings);
        Task<bool> ClearSettings(string id);
    }
}
