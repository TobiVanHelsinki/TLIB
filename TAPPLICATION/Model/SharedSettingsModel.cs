using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using TAPPLICATION.Settings;

namespace TAPPLICATION.Model
{
    public class SharedSettingsModel : INotifyPropertyChanged
    {
        public static IPlatformSettings PlatformSettings =
#if __ANDROID__
            new DroidSettings()
#else
            new WinSettings()
#endif
;
        #region Settinsg
        public bool InternSync
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC, value);
                Instance.NotifyPropertyChanged();
            }
        }
        public void InternSyncReset()
        {
            PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC, SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC_STD);
        }


        public bool BETA_FEATURES
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES, value);
                Instance.NotifyPropertyChanged();
            }
        }
        public void BETA_FEATURESReset()
        {
            PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES, SharedConstants.CONTAINER_SETTINGS_BETA_FEATURES_STD);
        }

        public bool DISPLAY_REQUEST
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST, value);
                Instance.NotifyPropertyChanged();
            }
        }
        public void DISPLAY_REQUESTReset()
        {
            PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST, SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST_STD);
        }

        public bool ORDNERMODE
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE, value);
                Instance.NotifyPropertyChanged();
            }
        }
        public void ORDNERMODEReset()
        {
            PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE, SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_STD);
        }

        public string ORDNERMODE_PFAD
        {
            get => PlatformSettings.getString(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH, value);
                NotifyPropertyChanged();
            }
        }
        public void ORDNERMODE_PFADReset()
        {
            PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH, SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH_STD);
        }

        #endregion
        #region Methods
        public void ResetAllSettings()
        {
            MethodInfo[] method = this.GetType().GetMethods();
            foreach (var item in method)
            {
                if (item.Name.Contains("Reset"))
                {
                    if (item.Name == "ResetAllSettings")
                    {
                        continue;
                    }
                    object result = item.Invoke(this, null);
                }
            }
        }
        #endregion
        #region Singleton Model Thigns

        public static SharedSettingsModel Initialize()
        {
            if (instance == null)
            {
                instance = new SharedSettingsModel();
            }
            return Instance;
        }
        public static SharedSettingsModel Instance
        {
            get
            {
                return instance;
            }
        }

        public static SharedSettingsModel I
        {
            get
            {
                return instance;
            }
        }

        protected static SharedSettingsModel instance;


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
