using System;
using System.Collections.Generic;
using System.Text;

namespace TLIB.Code.Uwp
{
    public class UwpModelHelper : IModelHelper
    {
        public async void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
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
