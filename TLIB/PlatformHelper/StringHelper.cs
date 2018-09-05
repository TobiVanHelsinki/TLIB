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

        string GetSimpleCountryCode(string[] filter, string fallback);

        string GetPrefix(PrefixType type);
    }

    public static class StringHelper
    {
        public static IStringHelper Platform { get; set; }
        public static string GetString(string strID) => Platform.GetString(strID);

        public static List<string> GetStrings(string strID) => Platform.GetStrings(strID);
        public static string GetSimpleCountryCode(string[] filter, string fallback) => Platform.GetSimpleCountryCode(filter, fallback);
        public static string GetPrefix(PrefixType type) => Platform.GetPrefix(type);
    }
}
