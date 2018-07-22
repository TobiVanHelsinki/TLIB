using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLIB
{
    public static class CollectionHelper
    {
        public static bool TrueForAll<TSource>(this IEnumerable<TSource> source, Predicate<TSource> match)
        {
            if (match == null)
            {
                throw new System.ArgumentNullException();
            }
            return source.Aggregate(true, (b, i) => b && match.Invoke(i));
        }
        public static IEnumerable<IEnumerable<TSource>> CartesianProduct<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        {
            IEnumerable<IEnumerable<TSource>> emptyProduct = new[] { Enumerable.Empty<TSource>() };
            return source.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }
        /// <summary>
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

        public static TSource RandomElement<TSource>(this IEnumerable<TSource> source)
        {
            return source.ElementAtOrDefault(StaticRandom.Next(0, source.Count()));
        }
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
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> param)
        {
            foreach (var item in param)
            {
                source.Add(item);
            }
        }
        public static int MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            try
            {
                return source.Max(selector);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double MaxOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            try
            {
                return source.Max(selector);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static int MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
        {
            try
            {
                return source.Min(selector);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static double MinOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, double> selector)
        {
            try
            {
                return source.Min(selector);
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
