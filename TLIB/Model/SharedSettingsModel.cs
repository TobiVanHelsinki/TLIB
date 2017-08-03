using System.ComponentModel;
using System.Runtime.CompilerServices;
using TLIB.Settings;

namespace TLIB.Model
{
    public class SharedSettingsModel : INotifyPropertyChanged
    {
        public IPlatformSettings PlatformSettings =
#if __ANDROID__
            new DroidSettings()
#else
            new WinSettings()
#endif
;

        public bool InternSync
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_INTERN_SYNC, value);
                Instance.NotifyPropertyChanged();
            }
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

        public bool DISPLAY_REQUEST
        {
            get => PlatformSettings.getBool(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_DISPLAY_REQUEST, value);
                Instance.NotifyPropertyChanged();
            }
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
        public string ORDNERMODE_PFAD
        {
            get => PlatformSettings.getString(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH);
            set
            {
                PlatformSettings.set(SharedConstants.CONTAINER_SETTINGS_FOLDERMODE_PATH, value);
                NotifyPropertyChanged();
            }
        }

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
    }
}
