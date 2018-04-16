using System;
using System.Collections.Generic;

namespace TAMARIN.Settings
{
    public class WinSettings : IPlatformSettings
    {
        Windows.Storage.ApplicationDataContainer Settings = Windows.Storage.ApplicationData.Current.LocalSettings;

        public void set(string place, object value)
        {
            try
            {
                Settings.Values[place] = value;
            }
            catch (Exception)
            {
            }
        }
        public bool getBool(string place, bool fallback = default)
        {
            try
            {
                return bool.Parse(Settings.Values[place].ToString());
            }
            catch (Exception)
            {
            }
            return fallback;
        }
        public string getString(string place, string fallback = default)
        {
            try
            {
                return Settings.Values[place].ToString();
            }
            catch (Exception)
            {
            }
            return fallback;
        }

        public int getInt(string place, int fallback = default)
        {
            try
            {
                return int.Parse(Settings.Values[place].ToString());
            }
            catch (Exception)
            {
            }
            return fallback;
        }

        public IEnumerable<T> getIEnumerable<T>(string place, T fallback = default)
        {
            var val = Settings.Values[place];
            throw new NotImplementedException();
        }
    }
}
