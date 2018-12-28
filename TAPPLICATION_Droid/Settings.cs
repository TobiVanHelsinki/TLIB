using System;
using TLIB.Settings;

namespace TAPPLICATION_Droid
{
    internal class Settings : IPlatformSettings
    {
        public void SetLocal(string place, object value)
        {
            return;
            throw new ArgumentException();
        }
        public void SetRoaming(string place, object value)
        {
            return;
            throw new ArgumentException();
        }

        public object GetLocal(string place)
        {
            return default;
            throw new ArgumentException();
        }
        public object GetRoaming(string place)
        {
            return default;
            throw new ArgumentException();
        }
    }
}
