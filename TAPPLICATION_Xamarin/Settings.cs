using System;
using TLIB.Settings;

namespace TAPPLICATION_Xamarin
{
    internal class Settings : IPlatformSettings
    {
        public void SetLocal(string place, object value)
        {
            throw new ArgumentException();
        }
        public void SetRoaming(string place, object value)
        {
            throw new ArgumentException();
        }

        public object GetLocal(string place)
        {
            throw new ArgumentException();
        }
        public object GetRoaming(string place)
        {
            throw new ArgumentException();
        }
    }
}
