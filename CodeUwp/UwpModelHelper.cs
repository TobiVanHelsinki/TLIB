using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TLIB.PlatformHelper;
using Windows.ApplicationModel.Core;

namespace TLIB.Code.Uwp
{
    public class UwpModelHelper : IModelHelper
    {
        public async Task CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
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

        public void ExecuteOnUIThreadAsync(Action p)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(p);
        }
    }
}
