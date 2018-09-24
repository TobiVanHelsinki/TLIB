using System;
using System.Collections.Generic;
using TLIB.Settings;

namespace TLIB.Code.Android
{
    public class Settings : IPlatformSettings
    {
        public bool GetBoolLocal(string place, bool fallback = false)
        {
            return fallback;
        }

        public bool GetBoolRoaming(string place, bool fallback = false)
        {
            return fallback;
        }

        public IEnumerable<T> GetIEnumerable<T>(string place, T fallback = default)
        {
            return new List<T>();
        }

        public int GetIntLocal(string place, int fallback = 0)
        {
            return fallback;
        }

        public int GetIntRoaming(string place, int fallback = 0)
        {
            return fallback;
        }

        public string GetStringLocal(string place, string fallback = "")
        {
            return fallback;
        }

        public string GetStringRoaming(string place, string fallback = null)
        {
            return fallback;
        }

        public void SetLocal(string place, object value)
        {
        }

        public void SetRoaming(string place, object value)
        {
        }
    }
}
