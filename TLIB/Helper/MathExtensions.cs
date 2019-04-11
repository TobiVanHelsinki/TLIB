using System;

namespace TLIB
{
    public static class MathExtensions
    {
        /// <summary>
        /// returns the lower of two numbers
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static uint Min(this uint A, uint B)
        {
            return A < B ? A : B;
        }
        /// <summary>
        /// returns the lower of two elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static T Min<T>(this T A, T B) where T : IComparable<T>
        {
            return A.CompareTo(B) < 0 ? A : B; //TODO Eval
        }

        /// <summary>
        /// Return the lower of two numbers or Fallback, if specified and toTest is higher then Border
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <param name="Fallback"></param>
        /// <returns></returns>
        public static int UpperB(this int toTest, int Border, int? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }

        /// <summary>
        /// Return the upper of two numbers or Fallback, if specified and toTest is lower then Border
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <param name="Fallback"></param>
        /// <returns></returns>
        public static int LowerB(this int toTest, int Border, int? Fallback = null)
        {
            return toTest >= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static uint UpperB(this uint toTest, uint Border, uint? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static uint LowerB(this uint toTest, uint Border, uint? Fallback = null)
        {
            return toTest >= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static float UpperB(this float toTest, float Border, float? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static float LowerB(this float toTest, float Border, float? Fallback = null)
        {
            return toTest >= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static double UpperB(this double toTest, double Border, double? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static double LowerB(this double toTest, double Border, double? Fallback = null)
        {
            return toTest >= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static decimal UpperB(this decimal toTest, decimal Border, decimal? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
        public static decimal LowerB(this decimal toTest, decimal Border, decimal? Fallback = null)
        {
            return toTest >= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }

        public static double Pow(this double Base, double Exponent)
        {
            return Math.Pow(Base, Exponent);
        }
        public static int Pow(this int Base, int Exponent)
        {
            return (int)Math.Pow(Base, Exponent);
        }
        public static uint Pow(this uint Base, uint Exponent)
        {
            return (uint)Math.Pow(Base, Exponent);
        }
    }
}
