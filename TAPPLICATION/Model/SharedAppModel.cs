using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TAPPLICATION.Model
{
    public class SharedAppModel : INotifyPropertyChanged
    {
        public ObservableCollection<Notification> lstNotifications = new ObservableCollection<Notification>();
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PlatformHelper.CallPropertyChanged(PropertyChanged, this, propertyName);
        }

        public void NewNotification(Notification Not)
        {
            PlatformHelper.ExecuteOnUIThreadAsync(() =>
            {
                lstNotifications.Insert(0, Not);
            });
        }

        public void NewNotification(string Message)
        {
            NewNotification(new Notification(Message));
        }

        public void NewNotification(string Message, Exception ex)
        {
            NewNotification(new Notification(Message) { ThrownException = ex });
        }

        public void NewNotification(string Message, bool Ligth)
        {
            NewNotification(new Notification(Message) { IsLight = Ligth });
        }

        public void NewNotification(string Message, int time)
        {
            NewNotification(new Notification(Message) { ShownTime = time });
        }

        public void NewNotification(string Message, bool Ligth, int time)
        {
            NewNotification(new Notification(Message) { IsLight = Ligth, ShownTime = time });
        }

        public void NewNotification(string Message, Exception ex, bool Ligth)
        {
            NewNotification(new Notification(Message) { ThrownException = ex, IsLight = Ligth });
        }

        public void NewNotification(string Message, Exception ex, int time)
        {
            NewNotification(new Notification(Message) { ThrownException = ex, ShownTime = time });
        }

        public void NewNotification(string Message, Exception ex, bool Ligth, int time)
        {
            NewNotification(new Notification(Message) { ThrownException = ex, IsLight = Ligth, ShownTime = time });
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
                    //if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                    var T = IO.SharedIO.SaveAtOriginPlace(MainObject);
                    T.Wait();
                    SharedSettingsModel.I.LAST_SAVE_INFO = T.Result;
                    MainObjectSaved?.Invoke(this, MainObject);
                    System.Diagnostics.Debug.WriteLine("MainObject Saved " + MainObject.ToString());
                }
                catch (Exception ex)
                {
                    TAPPLICATION.Debugging.TraceException(ex);
                    System.Diagnostics.Debug.WriteLine("Error Saving the MainObject " + MainObject.ToString() + ex.Message);
                    try
                    {
                        NewNotification("Error saving Char", ex, 2);
                    }
                    catch (Exception ex2)
                    {
                        TAPPLICATION.Debugging.TraceException(ex2);
                    }
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        //System.Diagnostics.Debugger.Break();
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
