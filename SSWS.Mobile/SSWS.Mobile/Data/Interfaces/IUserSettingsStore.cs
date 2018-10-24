namespace SSWS.Mobile.Data.Interfaces
{
    public interface IUserSettingsStore
    {
        UserSettings LoadSettings(string id);
        bool SaveSettings(string id, UserSettings settings);
    }
}
