namespace SSWS.Mobile.Data.Interfaces
{
    public interface IUserIdProvider
    {
        bool Exists();

        void Set(string id);

        string Get();

        void Clear();
    }
}
