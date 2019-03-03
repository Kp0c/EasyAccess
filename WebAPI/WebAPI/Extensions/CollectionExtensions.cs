using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class CollectionExtensions
    {
        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            IEnumerable<T> itemsToRemove = collection.Where(predicate);

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
