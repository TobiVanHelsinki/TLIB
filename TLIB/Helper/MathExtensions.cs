using System;

namespace TLIB
{
    /// <summary>
    /// Extension methods for IComparable Types, aka Numbers
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// returns the lower of two elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static T Min<T>(this T A, T B) where T : IComparable<T>
        {
            return A.CompareTo(B) < 0 ? A : B;
        }

        /// <summary>
        /// returns the higher of two elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static T Max<T>(this T A, T B) where T : IComparable<T>
        {
            return A.CompareTo(B) > 0 ? A : B;
        }

        /// <summary>
        /// Return the lower of two elements
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <returns></returns>
        public static T UpperB<T>(this T toTest, T Border) where T : IComparable<T>
        {
            return toTest.CompareTo(Border) < 0 ? toTest : Border;
        }

        /// <summary>
        /// Returns the lower of two numbers or Fallback, if toTest is not lower then Border
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <param name="Fallback"></param>
        /// <returns></returns>
        public static T UpperB<T>(this T toTest, T Border, T Fallback) where T : IComparable<T>
        {
            return toTest.CompareTo(Border) < 0 ? toTest : Fallback;
        }

        /// <summary>
        /// Return the higher of two elements
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <returns></returns>
        public static T LowerB<T>(this T toTest, T Border) where T : IComparable<T>
        {
            return toTest.CompareTo(Border) > 0 ? toTest : Border;
        }

        /// <summary>
        /// Returns the higher of two numbers or Fallback, if toTest is not higher then Border
        /// </summary>
        /// <param name="toTest"></param>
        /// <param name="Border"></param>
        /// <param name="Fallback"></param>
        /// <returns></returns>
        public static T LowerB<T>(this T toTest, T Border, T Fallback) where T : IComparable<T>
        {
            return toTest.CompareTo(Border) > 0 ? toTest : Fallback;
        }

        /// <summary>
        /// See Math.Pow
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Exponent"></param>
        /// <returns></returns>
        public static double Pow(this double Base, double Exponent)
        {
            return Math.Pow(Base, Exponent);
        }

        /// <summary>
        /// See Math.Pow
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Exponent"></param>
        /// <returns>the double from Math.Pow cast to int</returns>
        public static int Pow(this int Base, int Exponent)
        {
            return (int)Math.Pow(Base, Exponent);
        }

        /// <summary>
        /// See Math.Pow
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Exponent"></param>
        /// <returns>the double from Math.Pow cast to uint</returns>
        public static uint Pow(this uint Base, uint Exponent)
        {
            return (uint)Math.Pow(Base, Exponent);
        }
    }
}
