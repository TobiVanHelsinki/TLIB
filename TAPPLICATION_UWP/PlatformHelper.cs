using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TAPPLICATION;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

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
                    Task T = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    }).AsTask();
                    T.Wait();
                }
                catch (Exception)
                {
                }
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            Task T = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        p?.Invoke();
                    }).AsTask();
            T.Wait();
        }
    }
}
