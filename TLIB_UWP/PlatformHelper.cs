using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TLIB;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;

namespace TLIB_UWP
{
    internal class PlatformHelper : IPlatformHelper
    {
        public string GetString(string strID)
        {
            try
            {
                return ResourceLoader.GetForViewIndependentUse().GetString(strID);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
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
