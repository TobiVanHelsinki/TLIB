using System;
using System.ComponentModel;

namespace TAPPLICATION
{
    public interface IPlatformHelper
    {
        void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property);
        void ExecuteOnUIThreadAsync(Action p);
    }

    public static class PlatformHelper
    {
        public static IPlatformHelper Platform { get; set; }
        static IPlatformHelper StandardPlatform = new StandardPlatformHelper();

        public static void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
            if (Platform != null)
            {
                Platform.CallPropertyChanged(Event, o, property);
            }
            else
            {
                StandardPlatform.CallPropertyChanged(Event, o, property);
            }
        }

        public static void ExecuteOnUIThreadAsync(Action p)
        {
            if (Platform != null)
            {
                Platform.ExecuteOnUIThreadAsync(p);
            }
            else
            {
                StandardPlatform.ExecuteOnUIThreadAsync(p);
            }
        }
    }

    class StandardPlatformHelper : IPlatformHelper
    {
        public void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property)
        {
            try
            {
                Event?.Invoke(o, new PropertyChangedEventArgs(property));
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
            }
        }

        public void ExecuteOnUIThreadAsync(Action p)
        {
        }
    }
}
