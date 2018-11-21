using System;
using System.ComponentModel;
using TLIB;

namespace TLIB_CORE
{
    internal class PlatformHelper : IPlatformHelper
    {
        public string GetString(string strID)
        {
            return "NotSupported by this TLIB Implementation";
        }
        public void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
            try
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
            }
            catch (Exception)
            {
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            try
            {
                p?.Invoke();
            }
            catch (Exception)
            {
            }
        }
    }
}
