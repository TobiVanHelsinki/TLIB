using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TAPPLICATION;
using Windows.ApplicationModel.Core;

namespace TAPPLICATION_UWP
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
                    Task T = DispatcherHelper.AwaitableRunAsync(CoreApplication.MainView?.Dispatcher, () =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    });
                    T.Wait();
                }
                catch (Exception)
                {
                }
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(p);
        }
    }
}
