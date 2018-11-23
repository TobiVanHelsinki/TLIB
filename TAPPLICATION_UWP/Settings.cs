using System;
using TLIB.Settings;
using Windows.Storage;

namespace TAPPLICATION_UWP
{
    internal class Settings : IPlatformSettings
    {
        ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        ApplicationDataContainer RoamingSettings = ApplicationData.Current.RoamingSettings;

        public void SetLocal(string place, object value)
        {
            try
            {
                LocalSettings.Values[place] = value;
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
        public void SetRoaming(string place, object value)
        {
            try
            {
                RoamingSettings.Values[place] = value;
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        public bool GetBoolLocal(string place, bool fallback = default)
        {
            try
            {
                return bool.Parse(LocalSettings.Values[place].ToString());
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                return fallback;
            }
        }
        public bool GetBoolRoaming(string place, bool fallback = false)
        {
            try
            {
                return bool.Parse(RoamingSettings.Values[place].ToString());
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                try
                {
                    return bool.Parse(LocalSettings.Values[place].ToString());
                }
                catch (Exception ex2)
 { TAPPLICATION.Debugging.TraceException(ex2);
                    return fallback;
                }
            }
        }
        public string GetStringLocal(string place, string fallback = default)
        {
            try
            {
                return LocalSettings.Values[place].ToString();
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                return fallback;
            }
        }
        public string GetStringRoaming(string place, string fallback = default)
        {
            try
            {
                return RoamingSettings.Values[place].ToString();
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                try
                {
                    return LocalSettings.Values[place].ToString();
                }
                catch (Exception ex2)
 { TAPPLICATION.Debugging.TraceException(ex2);
                    return fallback;
                }
            }
        }
        public int GetIntLocal(string place, int fallback = default)
        {
            try
            {
                return int.Parse(LocalSettings.Values[place].ToString());
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                return fallback;
            }
        }

        public int GetIntRoaming(string place, int fallback = 0)
        {
            try
            {
                return int.Parse(RoamingSettings.Values[place].ToString());
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
                try
                {
                    return int.Parse(LocalSettings.Values[place].ToString());
                }
                catch (Exception ex2)
 { TAPPLICATION.Debugging.TraceException(ex2);
                    return fallback;
                }
            }
        }
    }
}
