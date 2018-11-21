namespace TLIB_UWP
{
    public class Init
    {
        public static void Do()
        {
            TLIB.PlatformHelper.Platform = new PlatformHelper();
        }
    }
}
