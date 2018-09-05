using System;
using System.ComponentModel;
#if WINDOWS_UWP
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Core;
#endif

namespace TLIB
{
    public static class ModelHelper
    {
        public static async void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
#if WINDOWS_UWP
            try
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
            }
            catch (Exception)
            {
                try
                {
                    await DispatcherHelper.AwaitableRunAsync(CoreApplication.MainView?.Dispatcher, () =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    });
                }
                catch (Exception ex)
                {
                }
            }
#else
#endif
        }

        public static void ExecuteOnUIThreadAsync(Action p)
        {
#if WINDOWS_UWP
            DispatcherHelper.ExecuteOnUIThreadAsync(p);
#endif
        }
    }
}
