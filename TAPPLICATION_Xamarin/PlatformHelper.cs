using System;
using System.ComponentModel;
using TAPPLICATION;

namespace TAPPLICATION_Xamarin
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
            {
                TAPPLICATION.Debugging.TraceException(ex);
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            try
            {
                p?.Invoke();
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
            }
        }
    }
}
