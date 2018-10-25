using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LocalStorageUserIdProvider))]
namespace SSWS.Mobile.Data
{
    public class LocalStorageUserIdProvider : IUserIdProvider
    {
        public const string ApplicationPropertiesKey = "SSWS.UserId";

        public bool Exists()
        {
            IDictionary<string, object> storage = Application.Current.Properties;
            return storage.ContainsKey(ApplicationPropertiesKey);
        }

        public string Get()
        {
            if (!Exists())
            {
                throw new InvalidOperationException("The id for this instance is not set!");
            }
            IDictionary<string, object> storage = Application.Current.Properties;
            return storage[ApplicationPropertiesKey].ToString();
        }

        public void Set(string id)
        {
            if (Exists())
            {
                throw new InvalidOperationException("The id for this instance is already set!");
            }
            IDictionary<string, object> storage = Application.Current.Properties;
            storage[ApplicationPropertiesKey] = id;
        }

        public void Clear()
        {
            if (!Exists())
            {
                throw new InvalidOperationException("The id for this instance is not set!");
            }
            IDictionary<string, object> storage = Application.Current.Properties;
            storage.Remove(ApplicationPropertiesKey);
        }
    }
}
