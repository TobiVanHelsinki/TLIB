﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TLIB.Code.Uwp
{
    public class UwpStringHelper : IStringHelper
    {
        public string GetString(string strID)
        {
            return Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse().GetString(strID);
        }
        public static List<string> GetStrings(string strID)
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

        public static string GetSimpleCountryCode(string[] filter, string fallback)
        {
            return filter.Contains(CultureInfo.CurrentCulture.TwoLetterISOLanguageName) ? CultureInfo.CurrentCulture.TwoLetterISOLanguageName : fallback;
        }

        public static string GetPrefix(PrefixType type)
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