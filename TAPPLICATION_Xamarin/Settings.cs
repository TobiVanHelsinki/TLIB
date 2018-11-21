using System;
using TLIB.Settings;

namespace TAPPLICATION_Xamarin
{
    internal class Settings : IPlatformSettings
    {
        public bool GetBoolLocal(string place, bool fallback = false)
        {
            return default;
        }

        public bool GetBoolRoaming(string place, bool fallback = false)
        {
            return default;
        }

        public int GetIntLocal(string place, int fallback = 0)
        {
            return default;
        }

        public int GetIntRoaming(string place, int fallback = 0)
        {
            return default;
        }

        public string GetStringLocal(string place, string fallback = null)
        {
            return default;
        }

        public string GetStringRoaming(string place, string fallback = null)
        {
            return default;
        }

        public void SetLocal(string place, object value)
        {
        }

        public void SetRoaming(string place, object value)
        {
        }
    }
}
