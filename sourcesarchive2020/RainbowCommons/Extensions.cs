using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace System
{
    public static class Extensions
    {
        public static T RandomElement<T>(this IEnumerable<T> source)
        {
            return source.RandomElement(1).Single();
        }

        public static IEnumerable<T> RandomElement<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }


        public static IEnumerable<int> Enumerate(this int inp)
        {
            return Enumerable.Range(1, inp);
        }
    }
}