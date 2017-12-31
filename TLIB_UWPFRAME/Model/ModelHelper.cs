using System;
using System.ComponentModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace TLIB_UWPFRAME.Model
{
    class ModelHelper
    {
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

        public static async void AtGui(Action x, CoreDispatcherPriority Priority = CoreDispatcherPriority.Low)
        {
            //await Windows.UI.Xaml.Window.Current.Dispatcher.RunAsync(
            await SharedAppModel.Instance.Dispatcher.RunAsync(
                Priority, () => {
                    x();
                });
        }
    }
}
