using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TLIB.PlatformHelper;

namespace TLIB.Code.Android
{
    public class PlatformHelper : IPlatformHelper
    {

        public string GetString(string strID)
        {
            try
            {
                return "---";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
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
                    //await DispatcherHelper.AwaitableRunAsync(CoreApplication.MainView?.Dispatcher, () =>
                    //{
                    //    Event?.Invoke(o, new PropertyChangedEventArgs(property));
                    //});
                }
                catch (Exception ex)
                {
                }
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
            //DispatcherHelper.ExecuteOnUIThreadAsync(p);
        }
    }
}
