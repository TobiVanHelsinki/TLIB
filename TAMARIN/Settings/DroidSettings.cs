using System;
using System.Collections.Generic;

namespace TAMARIN.Settings
{
    public class DroidSettings : IPlatformSettings
    {
        public bool GetBoolLocal(string place, bool fallback = false)
        {
            throw new NotImplementedException();
        }

        public bool GetBoolRoaming(string place, bool fallback = false)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetIEnumerable<T>(string place, T fallback = default)
        {
            throw new NotImplementedException();
        }

        public int GetIntLocal(string place, int fallback = 0)
        {
            throw new NotImplementedException();
        }

        public int GetIntRoaming(string place, int fallback = 0)
        {
            throw new NotImplementedException();
        }

        public string GetStringLocal(string place, string fallback = "")
        {
            throw new NotImplementedException();
        }

        public string GetStringRoaming(string place, string fallback = null)
        {
            throw new NotImplementedException();
        }

        public void SetLocal(string place, object value)
        {
            throw new NotImplementedException();
        }

        public void SetRoaming(string place, object value)
        {
            throw new NotImplementedException();
        }
    }
}
