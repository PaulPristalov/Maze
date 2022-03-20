using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FirerusUtilities
{
    public static class CollectionExtensions
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            return array[Random.Range(0, array.Length)];
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static Collider2D GetNearest(this Collider2D[] array, Vector3 position)
        {
            if (array.Length == 0)
            {
                return null;
            }

            Collider2D nearest = array[0];

            if (array.Length > 1)
            {
                for (int i = 1; i < array.Length; i++)
                {
                    if (Vector3.Distance(position, nearest.transform.position) >
                        Vector3.Distance(position, array[i].transform.position))
                    {
                        nearest = array[i];
                    }
                }
            }

            return nearest;
        }

        public static TComponent[] GetComponent<T, TComponent>(this T[] array) 
            where T : Component 
            where TComponent : Component
        {
            List<TComponent> components = new List<TComponent>();
            for (int i = 0; i < array.Length; i++)
            {
                try
                {
                    components.Add(array[i].GetComponent<TComponent>());
                }
                catch
                {
                    continue;
                }
            }

            return components.ToArray();
        }

        public static int IndexOf<T>(this T[] collection, T element)
        {
            return System.Array.IndexOf(collection, element);
        }
        public static int IndexOf<T>(this List<T> collection, T element)
        {
            return System.Array.IndexOf(collection.ToArray(), element);
        }

        public static T GetElement<T>(this T[] collection, T element)
        {
            return collection[collection.IndexOf(element)];
        }

        public static T GetElement<T>(this List<T> collection, T element)
        {
            return collection[collection.IndexOf(element)];
        }

        public static int CountOfElements<T>(this IEnumerable<T> array, T element)
        {
            int result = 0;
            foreach (var el in array)
            {
                if (el.Equals(element))
                {
                    result++;
                }
            }
            return result;
        }

        public static T LastElement<T>(this T[] array)
        {
            return array[array.Length - 1];
        }

        public static T LastElement<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        public static T LastXElement<T>(this T[,] array, int y)
        {
            return array[array.GetLength(0) - 1, y];
        }

        public static T LastYElement<T>(this T[,] array, int x)
        {
            return array[x, array.GetLength(1) - 1];
        }

        public static T LastElement<T>(this T[,] array)
        {
            return array[array.GetLength(0) - 1, array.GetLength(1) - 1];
        }
    }
}
