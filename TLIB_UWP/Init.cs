namespace TLIB_UWP
{
    public class Init
    {
        public static void Do()
        {
            TLIB.PlatformHelper.ModelHelper.Platform = new ModelHelper();
            TLIB.PlatformHelper.StringHelper.Platform = new StringHelper();
        }
    }
}
