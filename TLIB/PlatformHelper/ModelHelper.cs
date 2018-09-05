using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TLIB.PlatformHelper
{
    public interface IModelHelper
    {
        Task CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property);

        void ExecuteOnUIThreadAsync(Action p);
    }
    public static class ModelHelper
    {
        public static IModelHelper Platform { get; set; }

        public static async void CallPropertyChanged(PropertyChangedEventHandler Event, object o, string property) => await Platform.CallPropertyChanged(Event, o, property);

        public static void ExecuteOnUIThreadAsync(Action p) => Platform.ExecuteOnUIThreadAsync(p);
    }
}
