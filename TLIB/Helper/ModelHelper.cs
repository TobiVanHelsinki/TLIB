#if WINDOWS_UWP
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
#endif

namespace TLIB
{
    public static class ModelHelper
    {
#if WINDOWS_UWP
        public static async void CallPropertyChangedAtDispatcher(PropertyChangedEventHandler Event, object o, string property, CoreDispatcherPriority Prio = CoreDispatcherPriority.Normal)
        {
            try
            {
                await Windows.UI.Xaml.Window.Current?.Dispatcher?.RunAsync(Prio,
                () =>
                {
                    Event?.Invoke(o, new PropertyChangedEventArgs(property));
                });
            }
            catch (Exception)
            {
                try
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Prio, () =>
                    {
                        Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    });
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.Write("Exception at property changed");
                }
            }
    }
#endif
#if WINDOWS_UWP
        public static CoreDispatcher CDispatcher;
#endif
#if WINDOWS_UWP
        public static async void AtGui(Action x, CoreDispatcherPriority Priority = CoreDispatcherPriority.Low)
        {

            try
            {
                await CDispatcher.RunAsync(Priority, () => x());
            }
            catch (Exception)
            {
                try
                {
                    await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(Priority, () => x());
                }
                catch (Exception)
                {
                }
            }
        }
#endif
    }
}
