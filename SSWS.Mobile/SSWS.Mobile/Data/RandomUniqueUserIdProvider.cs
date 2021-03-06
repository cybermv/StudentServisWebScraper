﻿using SSWS.Mobile.Data;
using SSWS.Mobile.Data.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

//[assembly: Xamarin.Forms.Dependency(typeof(RandomUniqueUserIdProvider))]
namespace SSWS.Mobile.Data
{
    public class RandomUniqueUserIdProvider : IUserIdProvider
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
                Set($"SSWS-{Guid.NewGuid()}");
            }
            IDictionary<string, object> storage = Application.Current.Properties;
            return storage[ApplicationPropertiesKey].ToString();
        }

        public void Set(string id)
        {
            IDictionary<string, object> storage = Application.Current.Properties;
            storage[ApplicationPropertiesKey] = id;
        }

        public void Clear()
        {
            IDictionary<string, object> storage = Application.Current.Properties;
            storage.Remove(ApplicationPropertiesKey);
        }
    }
}
