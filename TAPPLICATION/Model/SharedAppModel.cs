using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TLIB.IO;
using TLIB.PlatformHelper;

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
        public event EventHandler<IMainType> MainObjectSaved;

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
            void save(object sender2, IMainType MainObject)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("SharedAppModel_PropertyChanged save");
                    var T = IO.SharedIO.SaveAtOriginPlace(MainObject, eUD: UserDecision.ThrowError);
                    T.Wait();
                    SharedSettingsModel.I.LAST_SAVE_INFO = T.Result;
                    MainObjectSaved?.Invoke(this, MainObject);
                    System.Diagnostics.Debug.WriteLine("MainObject Saved " + MainObject.ToString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error Saving the MainObject " + MainObject.ToString() + ex.Message);
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }

            if (e.PropertyName == nameof(MainObject))
            {
                if (MainObject != null)
                {
                    SharedSettingsModel.I.LAST_SAVE_INFO = MainObject.FileInfo;
                    MainObject.SaveRequest += save;
                }
                else
                {
                    SharedSettingsModel.I.LAST_SAVE_INFO = null;
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
