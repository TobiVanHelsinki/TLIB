#if __ANDROID__
#else 
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TLIB
{
    public enum PrefixType
    {
        AppPackageData = 1,
        AppUserData = 2,
    }

    public static class StringHelper
    {
        public static string GetString(string strID)
        {
            string strReturn = "";
#if WINDOWS_UWP
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
        public static List<string> GetStrings(string strID)
        {
            List<string> ret = new List<string>();
#if WINDOWS_UWP
#if __ANDROID__
            strReturn = "NotImplemented";
#else
            string Current = "";
            int Counter = 1;
            Loop:
            Current = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(strID + Counter);
            if (!String.IsNullOrEmpty(Current))
            {
                ret.Add(Current);
                Counter++;
                goto Loop;
            }
#endif
#else
#endif
            return ret;
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
