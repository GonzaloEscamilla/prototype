using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace _Project.Scripts.Utilities
{
    public static class EnumerableExtension 
    {
        public static IEnumerable<TValue> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> target)
        {
            Random rand = new Random();

            List<TValue> values = Enumerable.ToList(target.Values);

            int size = target.Count; while (true)
            {
                yield return values[rand.Next(size)];
            }
        }

        public static IEnumerable<TValue> RandomValues<TValue>(this IList<TValue> target)
        {
            Random rand = new Random();

            int size = target.Count; while (true)
            {
                yield return target[rand.Next(size)];
            }
        }

        public static TValue GetRandom<TValue>(this IList<TValue> target)
        {
            Random rand = new Random();
            int size = target.Count;

            return target[rand.Next(size)];
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
