using System;
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
    }
}
