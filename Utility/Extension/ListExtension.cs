using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility.Extension
{
    public static class ListExtension
    {
        public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var query = source.Select(selector);
            return query.ToList();
        }
    }
}
