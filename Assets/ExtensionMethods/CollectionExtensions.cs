using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace UniExtension
{
    public static class CollectionExtensions
    {
        
        /// <summary>
        /// Return random element of any ICollection, if cannot get element from collection throw exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T GetRandomElement<T>(this ICollection<T> collection)
        {
            if (collection.Count == 0)
                throw new InvalidOperationException("Empty collection");

            var randomIndex = Random.Range(0, collection.Count);

            if (collection is IList<T> list)
                return list[randomIndex];

            var index = 0;

            foreach (var item in collection)
            {
                if (index == randomIndex)
                    return item;

                index++;
            }

            throw new InvalidOperationException("Cannot return any value");
        }

        /// <summary>
        /// Return true if can get random value from collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetRandomElement<T>(this ICollection<T> collection, out T result)
        {
            result = default;
            
            if (collection.Count == 0)
                return false;

            var randomIndex = Random.Range(0, collection.Count);

            if (collection is IList<T> list)
            {
                result = list[randomIndex];
                return true;
            }

            var index = 0;

            foreach (var item in collection)
            {
                if (index == randomIndex)
                {
                    result = item;
                    return true;
                }

                index++;
            }

            return false;
        }
    }
}
