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
#if WINDOWS_UWP
        public static async void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
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
        }
#endif
    }
}
