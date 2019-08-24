using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Extensions
{
    public static class CollectionExtensions
    {
        public static void RemoveWhere<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            ICollection<T> itemsToRemove = collection.Where(predicate).ToList();

            foreach (var item in itemsToRemove)
            {
                collection.Remove(item);
            }
        }
    }
}
