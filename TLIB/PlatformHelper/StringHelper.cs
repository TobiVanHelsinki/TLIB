using System.Collections.Generic;

namespace TLIB.PlatformHelper
{
    public enum PrefixType
    {
        AppPackageData = 1,
        AppUserData = 2,
    }

    public interface IStringHelper
    {
        string GetString(string strID);
        List<string> GetStrings(string strID);
    }

    public static class StringHelper
    {
        public static IStringHelper Platform { get; set; }
        public static string GetString(string strID) => Platform.GetString(strID);

        public static List<string> GetStrings(string strID) => Platform.GetStrings(strID);
    }
}
