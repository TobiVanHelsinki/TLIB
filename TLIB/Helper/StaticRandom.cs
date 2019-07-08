using System;
using System.Collections.Generic;
using System.Text;

namespace TLIB
{
    /// <summary>
    /// Provides static Random Methods, that can be used over a whole project
    /// </summary>
    public static class StaticRandom
    {
        /// <summary>
        /// the used Random object
        /// </summary>
        public static Random r = new Random(DateTime.Now.Second);

        /// <summary>
        /// @see Random.Next();
        /// </summary>
        /// <returns></returns>
        public static int Next()
        {
            return r.Next();
        }

        /// <summary>
        /// returns a renadom number from the given area
        /// </summary>
        /// <param name="minValueInclusiv">The inclusive lower limit of the random number returned.</param>
        /// <param name="maxValueExclusive">The exclusive upper limit of the random number returned. maxValuemust be greater than or equal to minValue.</param>
        /// <param name="not">a number that is excluded from result set</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">minValue is bigger then maxValue.</exception>
        public static int Next(int minValueInclusiv, int maxValueExclusive, int not = int.MaxValue)
        {
            int ret;
            do
            {
                try
                {
                    ret = r.Next(minValueInclusiv, maxValueExclusive);
                }
                catch (Exception)
                {
                    ret = default;
                }
            } while (ret == not);

            return ret;
        }

        /// <summary>
        /// @see Random.Next(int);
        /// </summary>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Next(int maxValue)
        {
            return r.Next(maxValue);
        }

        /// <summary>
        /// retrieve a random datetime between minInclusive and maxExclusive.
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxExclusive"></param>
        public static DateTime NextDateTime(DateTime minInclusive, DateTime maxExclusive)
        {
            return new DateTime(NextLong(minInclusive.Ticks, maxExclusive.Ticks));
        }

        /// <summary>
        /// Returns a random long number from min to max
        /// </summary>
        /// <param name="minInclusive"></param>
        /// <param name="maxExclusive"></param>
        /// <returns></returns>
        public static long NextLong(long minInclusive, long maxExclusive)
        {
            System.Diagnostics.Debug.WriteLine("minInclusive: {0}, maxExclusive: {1}", minInclusive, maxExclusive);
            long result = r.Next((int)(minInclusive >> 32), (int)(maxExclusive >> 32));
            result = result << 32;
            result = result | (uint)r.Next((int)minInclusive, (int)maxExclusive);
            return result;
        }

        /// <summary>
        /// see Random.NextBytes(byte[]);
        /// </summary>
        /// <param name="buffer"></param>
        public static void NextBytes(byte[] buffer)
        {
            r.NextBytes(buffer);
        }

        /// <summary>
        /// see Random.NextDouble();
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return r.NextDouble();
        }
    }
}
