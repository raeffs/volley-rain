using System;
using System.Linq;
using System.Collections.Generic;

namespace VolleyRain
{
    public static class EnumerableExtensions
    {
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}