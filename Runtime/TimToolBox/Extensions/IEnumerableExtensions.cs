using System.Collections.Generic;
using System.Linq;

namespace TimToolBox.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        ///  Prune out all the null entry and return a IEnumerable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> RemoveNulls<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null) return null;
            //prune out all the null entry and return a IEnumerable
            return enumerable.Where(x => x != null);
        }
    }
}