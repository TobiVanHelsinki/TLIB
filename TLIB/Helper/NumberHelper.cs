using System;
using System.Globalization;

namespace TLIB
{
    public static class NumberHelper
    {
        /// <summary>
        /// Converts a string to a double
        /// this string can contain simple formulas like "-6.5+3,4" (=-3.1)
        /// </summary>
        /// <param name="FormulaString"></param>
        /// <returns></returns>
        public static double CalcToDouble(string FormulaString, bool treatKommataAsPoints = false)
        {
            if (FormulaString == null)
            {
                return 0;
            }
            string Temp = "";
            double Ret = 0;
            FormulaString += "+";
            if (treatKommataAsPoints)
            {
                FormulaString = FormulaString.Replace(',', '.');
            }
            foreach (char item in FormulaString)
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
                            Ret += double.Parse(Temp, CultureInfo.InvariantCulture);
                        }
                        catch (Exception) { }
                        Temp = "";
                    }
                    Temp += item;
                }

            }
            return Ret;
        }
        /// <summary>
        /// see CalcToDouble
        /// </summary>
        /// <param name="FormulaString"></param>
        /// <param name="treatKommataAsPoints"></param>
        /// <returns></returns>
        public static int CalcToInt(string FormulaString, bool treatKommataAsPoints = false)
        {
            return (int)CalcToDouble(FormulaString, treatKommataAsPoints);
        }

    }
}
