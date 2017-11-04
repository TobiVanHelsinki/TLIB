using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace TLIB.Model
{
    public class SharedAppModel : INotifyPropertyChanged
    {
        public ObservableCollection<Notification> lstNotifications;
        public event PropertyChangedEventHandler PropertyChanged;
        protected async void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (null == Task.CurrentId)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                    () =>
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    });
            }
        }

        public void NewNotification(string Message, Exception x = null)
        {
            lstNotifications.Insert(0, new Notification(Message, x));
        }

        protected static SharedAppModel instance;
        public static SharedAppModel Instance
        {
            get
            {
                return instance;
            }
        }

    }
    public class SharedAppModel<MainType> : SharedAppModel where MainType : IMainType, new() //where Inheritor : SharedAppModel<MainType, Inheritor>, new()
    {
        public event EventHandler MainObjectSaved;

        protected static new SharedAppModel<MainType> instance;
        public static new SharedAppModel<MainType> Instance
        {
            get
            {
                return instance;
            }
        }
        MainType _MainObject;
        public MainType MainObject
        {
            get { return this._MainObject; }
            set
            {
                //if (value != _MainObject)
                if (value != null)
                {
                    if (!value.Equals(_MainObject))
                    {
                        this._MainObject = value;
                        NotifyPropertyChanged();
                    }
                }
                else
                {
                    if (!_MainObject.Equals(value))
                    {
                        this._MainObject = value;
                        NotifyPropertyChanged();
                    }
                }
            }
        }

        public SharedAppModel()
        {
            PropertyChanged += SharedAppModel_PropertyChanged;
            lstNotifications = new ObservableCollection<Notification>();
#if DEBUG
            //Exception EX1 = new Exception("Test 1");
            //EX1.Source = "ich";
            //Exception EX2 = new Exception("Test 2");
            //Exception EX3 = new Exception("Test 3");
            //Exception EX4 = new Exception("Test 4");
            //NewNotification("Not 0");
            //NewNotification("Not 1", EX1);
            //NewNotification("Not 2", EX2);
            //NewNotification("Not 3", EX3);
            //NewNotification("Not 4", EX4);
#endif
        }

        private void SharedAppModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            async void save(object sender2, EventArgs e2)
            {
                await IO.SharedIO<MainType>.SaveAtOriginPlace(MainObject, eUD: IO.UserDecision.ThrowError);
                MainObjectSaved?.Invoke(this, new EventArgs());
            }

            if (e.PropertyName.Contains("MainObject"))
            {
                if (MainObject != null)
                {
                    MainObject.SaveRequest += save;
                }
            }
        }


        public MainType NewMainType()
        {
            MainObject = new MainType();
            return MainObject;
        }

        public override string ToString()
        {
            return MainObject.ToString() + " " + base.ToString();
        }
    }
}
