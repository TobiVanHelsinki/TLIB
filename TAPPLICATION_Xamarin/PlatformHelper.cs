using System;
using System.ComponentModel;
using TAPPLICATION;
using Xamarin.Essentials;

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
            catch (Exception)
            {
                try
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    });
                }
                catch (Exception ex)
                {
                    TAPPLICATION.Debugging.TraceException(ex);
                }
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(p);
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
            }
        }
    }
}
