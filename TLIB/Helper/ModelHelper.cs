#if WINDOWS_UWP
using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
#endif

namespace TLIB
{
    public static class ModelHelper
    {
#if WINDOWS_UWP
        public static CoreDispatcher CDispatcher;
        public static async void CallPropertyChangedAtDispatcher(PropertyChangedEventHandler Event, object o, string property, CoreDispatcherPriority Prio = CoreDispatcherPriority.Normal)
        {
            if (Environment.CurrentManagedThreadId == 0)
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
                return;
            }
            CoreDispatcher C = Window.Current?.Dispatcher ?? CDispatcher;
            if (C == null)
            {
                try
                {
                    //TODO Multiple Views, hier ForEach CoreApplication.Views [...]
                    C = CoreApplication.GetCurrentView()?.CoreWindow?.Dispatcher;
                }
                catch (Exception) { }
            }
            if (C != null)
            {
                await C.RunAsync(Prio, () => Event?.Invoke(o, new PropertyChangedEventArgs(property)));
            }
            else
            {
#if DEBUG
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    //System.Diagnostics.Debugger.Break();
                }
                System.Diagnostics.Debug.Write("No Dispatcher at property changed");
#endif
            }
        }
        public static async void AtGui(Action x, CoreDispatcherPriority Priority = CoreDispatcherPriority.Low)
        {
            CoreDispatcher C = CDispatcher ?? Window.Current?.Dispatcher ?? CoreApplication.MainView?.CoreWindow?.Dispatcher ;
            if (C != null)
            {
                await CDispatcher.RunAsync(Priority, () => x());
            }
            else
            {
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
                System.Diagnostics.Debug.Write("No Dispatcher at property changed");
            }
        }
#endif
    }
}
