namespace Cspec.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static T Second<T>(this IEnumerable<T> theEnumerable)
        {
            return theEnumerable.Skip(1).First();
        }

        public static T Third<T>(this IEnumerable<T> theEnumerable)
        {
            return theEnumerable.Skip(2).First();
        }

        public static T Fourth<T>(this IEnumerable<T> theEnumerable)
        {
            return theEnumerable.Skip(3).First();
        }

        public static T Fifth<T>(this IEnumerable<T> theEnumerable)
        {
            return theEnumerable.Skip(4).First();
        }
    }
}