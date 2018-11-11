using System;
using System.Collections.Generic;
using TLIB.PlatformHelper;
using Windows.ApplicationModel.Resources;

namespace TLIB_UWP
{
    internal class StringHelper : IStringHelper
    {
        public string GetString(string strID)
        {
            try
            {
                return ResourceLoader.GetForViewIndependentUse().GetString(strID);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<string> GetStrings(string strID)
        {
            var ret = new List<string>();
            var Counter = 1;
            string Current;
            Loop:
            try
            {
                Current = ResourceLoader.GetForViewIndependentUse().GetString(strID + Counter);
            }
            catch (Exception)
            {
                Current = null;
            }
            if (!string.IsNullOrEmpty(Current))
            {
                ret.Add(Current);
                Counter++;
                goto Loop;
            }
            return ret;
        }

    }
}
