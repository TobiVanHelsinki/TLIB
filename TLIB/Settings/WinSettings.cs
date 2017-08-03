using System;
namespace TLIB.Settings
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
        public bool getBool(string place, bool fallback = false)
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
        public string getString(string place, string fallback = "")
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

        public int getInt(string place, int fallback = 0)
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
    }
}
