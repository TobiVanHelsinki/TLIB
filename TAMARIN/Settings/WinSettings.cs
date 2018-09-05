#if WINDOWS_UWP

using System;
using System.Collections.Generic;
using Windows.Storage;

namespace TAMARIN.Settings
{
    public class WinSettings : IPlatformSettings
    {
        ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public void SetLocal(string place, object value)
        {
            try
            {
                LocalSettings.Values[place] = value;
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
        public void SetRoaming(string place, object value)
        {
            try
            {
                RoamingSettings.Values[place] = value;
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

        public bool GetBoolLocal(string place, bool fallback = default)
        {
            try
            {
                return bool.Parse(LocalSettings.Values[place].ToString());
            }
            catch (Exception)
            {
            }
            return fallback;
        }
        public bool GetBoolRoaming(string place, bool fallback = false)
        {
            try
            {
                return bool.Parse(RoamingSettings.Values[place].ToString());
            }
            catch (Exception)
            {
                try
                {
                    return bool.Parse(LocalSettings.Values[place].ToString());
                }
                catch (Exception)
                {
                }
            }
            return fallback;
        }
        public string GetStringLocal(string place, string fallback = default)
        {
            try
            {
                return LocalSettings.Values[place].ToString();
            }
            catch (Exception)
            {
            }
            return fallback;
        }
        public string GetStringRoaming(string place, string fallback = default)
        {
            try
            {
                return RoamingSettings.Values[place].ToString();
            }
            catch (Exception)
            {
                try
                {
                    return LocalSettings.Values[place].ToString();
                }
                catch (Exception)
                {
                }
            }
            return fallback;
        }
        public int GetIntLocal(string place, int fallback = default)
        {
            try
            {
                return int.Parse(LocalSettings.Values[place].ToString());
            }
            catch (Exception)
            {
            }
            return fallback;
        }

        public int GetIntRoaming(string place, int fallback = 0)
        {
            try
            {
                return int.Parse(RoamingSettings.Values[place].ToString());
            }
            catch (Exception)
            {
                try
                {
                    return int.Parse(LocalSettings.Values[place].ToString());
                }
                catch (Exception)
                {
                }
            }
            return fallback;
        }

        public IEnumerable<T> GetIEnumerable<T>(string place, T fallback = default)
        {
            var val = LocalSettings.Values[place];
            throw new NotImplementedException();
        }

    }
}
#endif
