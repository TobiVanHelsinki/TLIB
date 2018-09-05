using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TAMARIN.IO;
using TLIB;

namespace TAPPLICATION.Model
{
    public class SharedAppModel : INotifyPropertyChanged
    {
        public ObservableCollection<Notification> lstNotifications = new ObservableCollection<Notification>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            ModelHelper.CallPropertyChanged(PropertyChanged, this, propertyName);
        }

        public void NewNotification(string Message, Exception x)
        {
            ModelHelper.ExecuteOnUIThreadAsync(() =>
            {
                lstNotifications.Insert(0, new Notification(Message, x));
            });
        }

        public void NewNotification(string Message, bool isLightNotification = true, int seconds = 6)
        {
            ModelHelper.ExecuteOnUIThreadAsync(() =>
            {
                lstNotifications.Insert(0, new Notification(Message) { IsLight = isLightNotification, ShownTime = seconds * 1000 });
            });
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
    }
    public class SharedAppModel<MainType> : SharedAppModel where MainType : IMainType, new()
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
                    SystemHelper.WriteLine("MainObject Saved");
#endif
                }
                catch (Exception)
                {
#if DEBUG
                    SystemHelper.WriteLine("Error Saving the MainObject");
#endif
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
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
