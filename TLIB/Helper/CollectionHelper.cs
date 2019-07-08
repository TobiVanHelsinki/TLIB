using System;
using System.Collections.Generic;
using System.Linq;

namespace TLIB
{
    /// <summary>
    /// Provides extension methods for IEnumerable
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// Is a given predicate true for all elements
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static bool TrueForAll<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            if (match == null)
            {
                throw new System.ArgumentNullException();
            }
            return source.Aggregate(true, (b, i) => b && match.Invoke(i));
        }

        /// <summary>
        /// calculates the power set for the given set
        /// works for source.Count() values 0-30, 190
        /// works not for source.Count() values 31-35, 191-200
        /// other not tested
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TSource>> ToPowerSet<TSource>(this IEnumerable<TSource> source)
        {
            //https://codereview.stackexchange.com/questions/51938/get-distinct-combinations-of-numbers
            var ret = from m in Enumerable.Range(0, 1 << source.Count())
                      select
                          from i in Enumerable.Range(0, source.Count())
                          where (m & (1 << i)) != 0
                          select source.ElementAt(i);
            int sourceCount = source.Count();
            double shouldCount = System.Math.Pow(2, sourceCount);
            long retCount = ret.Count();
            return shouldCount == retCount ? ret : throw new Exception("Wrong count after calculation");
        }

        /// <summary>
        /// returns a random element of a sequence or throws an exception when the sequence is empty
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TSource RandomElement<TSource>(this IEnumerable<TSource> source)
        {
            return source.ElementAtOrDefault(StaticRandom.Next(0, source.Count()));
        }

        /// <summary>
        /// returns a number of random elements of a sequence or throws an exception when there are to less elements
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="number">how many elements to return</param>
        /// <param name="AllowRepeatants">shall the set be unique?</param>
        /// <returns></returns>
        public static IEnumerable<TSource> RandomElements<TSource>(this IEnumerable<TSource> source, int number, bool AllowRepeatants = true)
        {
            var ret = new List<TSource>();

            if (source.Count() <= number)
            {
                ret.AddRange(source);
            }
            else if (!AllowRepeatants)
            {
                var templist = source.ToList();
                for (int i = 0; i < number; i++)
                {
                    var newelement = templist.RandomElement();
                    ret.Add(newelement);
                    templist.Remove(newelement);
                }
            }
            else
            {
                for (int i = 0; i < number; i++)
                {
                    ret.Add(source.RandomElement());
                }
            }
            return ret;
        }

        /// <summary>
        /// Adds multiple Elements to an ICollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> source, IEnumerable<T> param)
        {
            foreach (var item in param)
            {
                source.Add(item);
            }
            return source;
        }

        /// <summary>
        /// Returns the maximum Element of an set or default in case of an error
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TReturn MaxOrDefault<TSource, TReturn>(this IEnumerable<TSource> source, Func<TSource, TReturn> selector)
        {
            if (source.Count() == 0)
            {
                return default;
            }
            try
            {
                return source.Max(selector);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Returns the minimum Element of an set or default in case of an error
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TReturn MinOrDefault<TSource, TReturn>(this IEnumerable<TSource> source, Func<TSource, TReturn> selector)
        {
            if (source.Count() == 0)
            {
                return default;
            }
            try
            {
                return source.Min(selector);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
