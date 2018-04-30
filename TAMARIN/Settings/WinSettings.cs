using System;
using System.Collections.Generic;
using Windows.Storage;

namespace TAMARIN.Settings
{
    public class WinSettings : IPlatformSettings
    {
        ApplicationDataContainer Settings = ApplicationData.Current.LocalSettings;

        public void set(string place, object value)
        {
            try
            {
                Settings.Values[place] = value;
            }
            catch (Exception)
            {
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
#endif
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
