#if __ANDROID__
#else 
#endif

using System;
using System.Globalization;
using System.Linq;

namespace TLIB
{
    public static class StringHelper
    {
        public static string GetString(string strID)
        {
            string strReturn = "";
#if UWP
#if __ANDROID__
            strReturn = "NotImplemented";
#else
            strReturn = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(strID);
#endif
#else
            strReturn = null;
#endif
            return strReturn;
        }
        public static string GetSimpleCountryCode(string[] filter, string fallback)
        {
            string strReturn = "";
#if __ANDROID__
            strReturn = "NotImplemented";
#else
            strReturn = filter.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) ? CultureInfo.CurrentCulture.TwoLetterISOLanguageName : fallback;
#endif
            return strReturn;
        }
        public enum PrefixType
        {
            AppPackageData = 1,
            AppUserData = 2,
        }
        public static string GetPrefix(PrefixType type)
        {
            string strReturn = "";
#if __ANDROID__
            strReturn = "NotImplemented";
#else 
            switch (type)
            {
                case PrefixType.AppPackageData:
                    strReturn = "ms-appx:///";
                    break;
                case PrefixType.AppUserData:
                    strReturn = "ms-appdata:///";
                    break;
                default:
                    break;
            }
#endif
            return strReturn;
        }
    }
}
