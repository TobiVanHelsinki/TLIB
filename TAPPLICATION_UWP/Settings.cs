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
            {
                TAPPLICATION.Debugging.TraceException(ex);
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
            {
                TAPPLICATION.Debugging.TraceException(ex);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        public object GetLocal(string place)
        {
            if (LocalSettings.Values.ContainsKey(place))
            {
                return LocalSettings.Values[place];
            }
            else
            {
                throw new ArgumentException();
            }
        }
        public object GetRoaming(string place)
        {
            if (RoamingSettings.Values.ContainsKey(place))
            {
                return RoamingSettings.Values[place];
            }
            else if (LocalSettings.Values.ContainsKey(place))
            {
                return RoamingSettings.Values[place];
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
