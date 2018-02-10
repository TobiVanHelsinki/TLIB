using System;
using System.Collections.Generic;

namespace TLIB_UWPFRAME.Settings
{
    public class DroidSettings : IPlatformSettings
    {
        public bool getBool(string place, bool fallback = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> getIEnumerable<T>(string place, T fallback = default)
        {
            throw new NotImplementedException();
        }

        public int getInt(string place, int fallback = 0)
        {
            throw new NotImplementedException();
        }

        public string getString(string place, string fallback = "")
        {
            throw new NotImplementedException();
        }

        public void set(string place, object value)
        {
            throw new NotImplementedException();
        }
    }
}
