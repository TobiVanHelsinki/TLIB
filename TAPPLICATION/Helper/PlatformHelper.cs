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

        public static void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property) => Platform?.CallPropertyChanged(Event, o, property);

        public static void ExecuteOnUIThreadAsync(Action p) => Platform?.ExecuteOnUIThreadAsync(p);
    }
}
