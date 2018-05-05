using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TAMARIN.IO;
using TLIB;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace TAPPLICATION.Model
{
    public class SharedAppModel : INotifyPropertyChanged
    {
        public ObservableCollection<Notification> lstNotifications;
        public event PropertyChangedEventHandler PropertyChanged;
        protected async void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (null == Task.CurrentId)
            {
                ModelHelper.CallPropertyChangedAtDispatcher(PropertyChanged, this, propertyName);
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

        public void NewNotification(string Message, Exception x = null, bool isLightNotification = true)
        {
            lstNotifications.Insert(0, new Notification(Message, x) { IsLight = isLightNotification });
        }

        protected static SharedAppModel instance;
        public static SharedAppModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedAppModel();
                }
                return instance;
            }
        }

        public SharedAppModel()
        {
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

        public void SetDependencies(CoreDispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
        /// <summary>
        /// for future multithreading
        /// </summary>
        /// <param name="dispatcher"></param>
        public CoreDispatcher Dispatcher;
    }
    public class SharedAppModel<MainType> : SharedAppModel where MainType : IMainType, new() //where Inheritor : SharedAppModel<MainType, Inheritor>, new()
    {
        public event EventHandler MainObjectSaved;

        public static new SharedAppModel<MainType> Instance
        {
            get
            {
                return (SharedAppModel < MainType > )instance;
            }
        }
        MainType _MainObject;
        public MainType MainObject
        {
            get { return this._MainObject; }
            set
            {
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

        public SharedAppModel() : base()
        {
            PropertyChanged += SharedAppModel_PropertyChanged;
        }

        private void SharedAppModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            async void save(object sender2, EventArgs e2)
            {
                try
                {
                    await IO.SharedIO.SaveAtOriginPlace(MainObject, eUD: UserDecision.ThrowError);
                    MainObjectSaved?.Invoke(this, new EventArgs());
#if DEBUG
                    SystemHelper.WriteLine("Char Saved Internaly");
#endif
                }
                catch (Exception)
                {
#if DEBUG
                    SystemHelper.WriteLine("Error Saving Internaly");
#endif
                }
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
