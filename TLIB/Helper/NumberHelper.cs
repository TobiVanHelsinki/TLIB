using System;
using System.Globalization;

namespace TLIB
{
    public static class NumberHelper
    {
        /// <summary>
        /// Converts a string to a double
        /// this string can contain simple formulas like "-6.5+3,4" (=-3.1)
        /// 
        /// </summary>
        /// <param name="strOrigin"></param>
        /// <returns></returns>
        public static double CalcToDouble(string strOrigin, bool treatKommataAsPoints = false)
        {
            if (strOrigin == null)
            {
                return 0;
            }
            string strTemp = "";
            double dRetVal = 0;
            strOrigin += "+";
            if (treatKommataAsPoints)
            {
                strOrigin = strOrigin.Replace(',', '.');
            }
            foreach (char item in strOrigin)
            {
                //filter out letters or special chars
                if (char.IsNumber(item)
                        || char.IsDigit(item)
                        || char.IsSeparator(item)
                        || char.IsPunctuation(item)
                        || item == '-'
                        || item == '+'
                        )
                {
                    if (item == '-' || item == '+')
                    {
                        try
                        {
                            dRetVal += Double.Parse(strTemp, CultureInfo.InvariantCulture);
                        }
                        catch (Exception) { }
                        strTemp = "";
                    }
                    strTemp += item;
                }

            }
            return dRetVal;
        }

        public static int CalcToInt(string strOrigin, bool treatKommataAsPoints = false)
        {
            if (strOrigin == null)
            {
                return 0;
            }
            string strTemp = "";
            double dRetVal = 0;
            strOrigin += "+";
            if (treatKommataAsPoints)
            {
                strOrigin = strOrigin.Replace(',', '.');
            }
            foreach (char item in strOrigin)
            {
                //filter out letters or special chars
                if (char.IsNumber(item)
                        || char.IsDigit(item)
                        || char.IsSeparator(item)
                        || char.IsPunctuation(item)
                        || item == '-'
                        || item == '+'
                        )
                {
                    if (item == '-' || item == '+')
                    {
                        try
                        {
                            dRetVal += Double.Parse(strTemp, CultureInfo.DefaultThreadCurrentUICulture);
                        }
                        catch (Exception) { }
                        strTemp = "";
                    }
                    strTemp += item;
                }

            }
            return (int)dRetVal;
        }

    }
}
