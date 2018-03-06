﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLIB_UWPFRAME.Resources
{
    public static class CollectionExtension
    {
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
    }
}
