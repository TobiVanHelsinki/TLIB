using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using TLIB;

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
                return (SharedAppModel<MainType>)instance;
            }
        }
        MainType _MainObject;
        public MainType MainObject
        {
            get { return this._MainObjects.FirstOrDefault(); }
            set
            {

                if (value != null)
                {
                    AddMainObject(value);
                    //if (!value.Equals(_MainObject))
                    //{
                    //    this._MainObject = value;
                    //    NotifyPropertyChanged();
                    //}
                }
                else
                {
                    RemoveMainObject(value);
                    //if (!_MainObject.Equals(value))
                    //{
                    //    this._MainObject = value;
                    //    NotifyPropertyChanged();
                    //}
                }
            }
        }

        List<MainType> _MainObjects = new List<MainType>();
        public IEnumerable<MainType> MainObjects => _MainObjects;

        public void AddMainObject(MainType sender)
        {
            if (!_MainObjects.Contains(sender))
            {
                _MainObjects.Add(sender);
                sender.SaveRequest += SaveMainType;
                NotifyPropertyChanged(nameof(MainObject));
            }
        }

        public void RemoveMainObject(MainType sender)
        {
            if (_MainObjects.Contains(sender))
            {
                _MainObjects.Remove(sender);
                sender.SaveRequest -= SaveMainType;
                NotifyPropertyChanged(nameof(MainObject));
            }
        }

        public SharedAppModel() : base()
        {
            PropertyChanged += SharedAppModel_PropertyChanged;
        }

        private void SharedAppModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainObject))
            {
                if (MainObject != null)
                {
                    SharedSettingsModel.I.LAST_SAVE_INFO = MainObject.FileInfo;
                    //MainObject.SaveRequest += SaveMainType;
                }
                else
                {
                    SharedSettingsModel.I.LAST_SAVE_INFO = null;
                }
            }
        }


        public void SaveMainType(object sender2, IMainType MainObject)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("SharedAppModel_PropertyChanged save");
                //if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                var T = IO.SharedIO.SaveAtOriginPlace(MainObject);
                T.Wait();
                SharedSettingsModel.I.LAST_SAVE_INFO = T.Result;
                MainObjectSaved?.Invoke(MainObject, MainObject);
                System.Diagnostics.Debug.WriteLine("MainObject Saved " + MainObject.ToString());
            }
            catch (Exception ex)
            {
                TAPPLICATION.Debugging.TraceException(ex);
                System.Diagnostics.Debug.WriteLine("Error Saving the MainObject " + MainObject.ToString() + ex.Message);
                try
                {
                    Log.Write("Error saving Char", ex);
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
