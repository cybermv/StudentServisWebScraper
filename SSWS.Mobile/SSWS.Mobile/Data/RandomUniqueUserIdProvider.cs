using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(RandomUniqueUserIdProvider))]
namespace SSWS.Mobile.Data
{
    public class RandomUniqueUserIdProvider : IUserIdProvider
    {
        public const string ApplicationPropertiesKey = "SSWS.UserId";

        public string GetUserId()
        {
            IDictionary<string, object> storage = Application.Current.Properties;

            if (!storage.ContainsKey(ApplicationPropertiesKey))
            {
                storage[ApplicationPropertiesKey] = $"SSWS-{Guid.NewGuid()}";
            }

            return storage[ApplicationPropertiesKey].ToString();
        }
    }
}
