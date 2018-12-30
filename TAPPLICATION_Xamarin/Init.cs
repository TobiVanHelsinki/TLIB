namespace TAPPLICATION_Xamarin
{
    public class Init
    {
        public static void Do()
        {
            TAPPLICATION.IO.SharedIO.CurrentIO = new IO();
            TAPPLICATION.Model.SharedSettingsModel.PlatformSettings = new Settings();
            TAPPLICATION.PlatformHelper.Platform = new PlatformHelper();
        }
    }
}
