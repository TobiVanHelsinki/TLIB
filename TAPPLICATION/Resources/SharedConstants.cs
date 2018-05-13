namespace TAPPLICATION
{
    public class SharedConstants
    {

        #region TOKEN
        public const string STRING_APP_VERSION_NUMBER = "APP_VERSION_NUMBER";
        public const string STRING_FILE_VERSION_NUMBER = "FILE_VERSION_NUMBER";
        public const string ERROR_TOKEN = "ERROR";

        #endregion

        #region Speicher Container

        public const string INTERN_SAVE_CONTAINER = "Char_Store";
        public const string CONTAINER_SETTINGS = "Char_Settings";

        public const string ACCESSTOKEN_FOLDERMODE = "ACCESSTOKEN_FOLDERMODE";
        public const string ACCESSTOKEN_FILEACTIVATED = "ACCESSTOKEN_FILEACTIVATED";
        #endregion

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

        public const string CONTAINER_SETTINGS_LAST_APP_VERSION = "SETTINGS_LAST_APP_VERSION";
        public const string CONTAINER_SETTINGS_LAST_APP_VERSION_STD = "";

        #region AppStore Constants
        #region Shapes

        public const string APP_MORE_APPS_SHAPE = "ms-windows-store://publisher/?name=";
        public const string APP_STORE_LINK_SHAPE = "ms-windows-store://pdp/?productid=";
        public const string APP_STORE_REVIEW_LINK_SHAPE = "ms-windows-store://review/?ProductId=";
        public const string APP_CONTACT_MAILTO_SHAPE = "mailto:";

        #endregion

        #region SetByApp
        public static string APP_VERSION_BUILD_DELIM { get; set; }
        public static string APP_STORE_ID { get; set; }
        public static string APP_PUBLISHER_MAIL { get; set; }
        public static string APP_PUBLISHER { get; set; }
        #endregion

        public static string APP_STORE_LINK { get => APP_STORE_LINK_SHAPE + APP_STORE_ID ?? ""; }
        public static string APP_STORE_REVIEW_LINK { get => APP_STORE_REVIEW_LINK_SHAPE + APP_STORE_ID ?? ""; }
        public static string APP_MORE_APPS { get => APP_MORE_APPS_SHAPE + APP_PUBLISHER; }
        public static string APP_PUBLISHER_MAILTO { get => APP_CONTACT_MAILTO_SHAPE + APP_PUBLISHER_MAIL; }

        #endregion

    }
}
