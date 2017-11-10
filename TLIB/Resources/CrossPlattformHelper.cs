#if __ANDROID__
#else 
#endif

using System;

namespace TLIB
{
    public static class CrossPlatformHelper
    {
        public static string GetString(string strID)
        {
            string strReturn = "";
#if __ANDROID__
            strReturn = "NotImplemented";
#else 
            strReturn = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(strID);
#endif
            return strReturn;
        }
        public static string GetSimpleCountryCode()
        {
            string strReturn = "";
#if __ANDROID__
            strReturn = "NotImplemented";
#else 
            strReturn = "de"; //TODO
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
