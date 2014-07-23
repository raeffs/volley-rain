using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            return !source.Any(predicate);
        }
    }
}