#if __ANDROID__

#else
using Windows.ApplicationModel;
#endif

namespace TLIB
{
    public class SharedConstants
    {
        

        /// <summary>
        /// App Versionen
        /// </summary>
        /// 
        public static string APP_VERSION_BUILD = string.Format("{0}{1}{2}{3}",
#if __ANDROID__
            null;
#else
             Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
#endif
        public static string APP_VERSION_BUILD_DELIM = string.Format("{0}.{1}.{2}.{3}",
#if __ANDROID__
            null;
#else
             Package.Current.Id.Version.Major, Package.Current.Id.Version.Minor, Package.Current.Id.Version.Build, Package.Current.Id.Version.Revision);
#endif


        /// <summary>
        /// Variablen Namen fuer Versionen
        /// </summary>
        public const string STRING_APP_VERSION_NUMBER = "APP_VERSION_NUMBER";
        public const string STRING_FILE_VERSION_NUMBER = "FILE_VERSION_NUMBER";
        /// <summary>
        /// exact length of a version, as string at file
        /// </summary>
        public const int STRING_VERSION_LENGTH = 3;

        /// <summary>
        /// Die Anzahl Zeichen Zwischen der Variable der Nummer und der Nummer ansich
        /// </summary>
        internal const int JSON_FILE_GAP = 3;

        /// <summary>
        /// Speicher Constants
        /// </summary>

        /// <summary>
        /// Speicher Container
        /// </summary>
        public const string INTERN_SAVE_CONTAINER = "Char_Store";
        public const string CONTAINER_SETTINGS = "Char_Settings";

        #region Speicher Einstellungen
        public const string CONTAINER_SETTINGS_DISPLAY_REQUEST = "SETTINGS_DISPLAY_REQUEST";
        public const bool CONTAINER_SETTINGS_DISPLAY_REQUEST_STD = true;
        public const string CONTAINER_SETTINGS_BETA_FEATURES = "SETTINGS_BETA_FEATURES";
        public const bool CONTAINER_SETTINGS_BETA_FEATURES_STD = false;
        public const string CONTAINER_SETTINGS_INTERN_SYNC = "SETTINGS_INTERN_SYNC";
        public const bool CONTAINER_SETTINGS_INTERN_SYNC_STD = true;
        
        public const string CONTAINER_SETTINGS_FOLDERMODE = "SETTINGS_FOLDERMODE";
        public const bool CONTAINER_SETTINGS_FOLDERMODE_STD = false;
        public const string CONTAINER_SETTINGS_FOLDERMODE_PATH = "SETTINGS_FOLDERMODE_PATH";
        public const string CONTAINER_SETTINGS_FOLDERMODE_PATH_STD = "";
        #endregion

        public const string AUTHOR = "Tobi van Helsinki";
        public const string APP_CONTACT_MAIL = "TobiVanHelsik@live.de";

        public const string APP_CONTACT_MAILTO = "mailto:TobiVanHelsinki @live.de";
        public const string APP_MORE_APPS = "ms-windows-store://publisher/?name=Tobi van Helsinki";

        public const string ACCESSTOKEN_FOLDERMODE = "ACCESSTOKEN_FOLDERMODE";
        public const string ACCESSTOKEN_FILEACTIVATED = "ACCESSTOKEN_FILEACTIVATED";


    }
}
