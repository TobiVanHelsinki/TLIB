using System;
using System.ComponentModel;
using TLIB;

namespace TLIB_Xamarin
{
    internal class PlatformHelper : IPlatformHelper
    {
        public void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
            try
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            try
            {
                p?.Invoke();
            }
            catch (Exception ex)
 { TAPPLICATION.Debugging.TraceException(ex);
            }
        }

        public string GetString(string strID)
        {
            return "Not Supported yet";
        }
    }
}
