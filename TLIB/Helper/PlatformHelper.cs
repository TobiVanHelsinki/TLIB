using System;
using System.ComponentModel;

namespace TLIB
{
    public interface IPlatformHelper
    {
        void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property);
        void ExecuteOnUIThreadAsync(Action p);
        string GetString(string strID);
    }

    public static class PlatformHelper
    {
        public static IPlatformHelper Platform { get; set; }
        public static string GetString(string strID) => Platform?.GetString(strID);

        public static void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property) => Platform?.CallPropertyChanged(Event, o, property);

        public static void ExecuteOnUIThreadAsync(Action p) => Platform?.ExecuteOnUIThreadAsync(p);
    }
}
