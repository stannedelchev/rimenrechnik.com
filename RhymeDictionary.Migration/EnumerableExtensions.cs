using System.Collections.Generic;
using System.Linq;

namespace RhymeDictionary.Migration
{
    internal static class EnumerableExtensions
    {
        public class EnumerationProgress<T>
        {
            public int Position { get; init; }

            public int Total { get; init; }

            public T Item { get; init; }
        }

        public static IEnumerable<EnumerationProgress<T>> WithProgress<T>(this IEnumerable<T> enumerable, int total = 0)
        {
            var position = 1;

            if (enumerable is IList<T> list)
            {
                total = list.Count;
            }

            foreach (var item in enumerable)
            {
                yield return new EnumerationProgress<T>()
                {
                    Position = position,
                    Total = total,
                    Item = item,
                };

                position++;
            }
        }
    }
}
