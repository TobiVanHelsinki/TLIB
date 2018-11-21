using System;
using System.ComponentModel;
using TLIB;

namespace TLIB_Xamarin
{
    internal class PlatformHelper : IPlatformHelper
    {
        public void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
        }

        public string GetString(string strID)
        {
            return "";
        }
    }
}
