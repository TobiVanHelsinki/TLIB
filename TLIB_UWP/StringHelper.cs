using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TLIB.PlatformHelper;

namespace TLIB_UWP
{
    internal class StringHelper : IStringHelper
    {
        public string GetString(string strID)
        {
            try
            {
                return Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(strID);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<string> GetStrings(string strID)
        {
            List<string> ret = new List<string>();
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
            return ret;
        }

        public string GetSimpleCountryCode(string[] filter, string fallback)
        {
            return filter.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) ? CultureInfo.CurrentCulture.TwoLetterISOLanguageName : fallback;
        }

        public string GetPrefix(PrefixType type)
        {
            string strReturn = "";

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
            return strReturn;
        }
    }
}
