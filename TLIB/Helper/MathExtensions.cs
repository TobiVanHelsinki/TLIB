using System;

namespace TLIB
{
    public static class MathExtensions
    {
        public static uint Min(this uint A, uint B)
        {
            return A < B ? A : B;
        }
        public static int UpperB(this int toTest, int Border, int? Fallback = null)
        {
            return toTest <= Border ? toTest : (Fallback != null ? Fallback.Value : Border);
        }
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
