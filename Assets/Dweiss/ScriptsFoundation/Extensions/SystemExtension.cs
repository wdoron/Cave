using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace System
{
    public delegate void Action<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public static class SystemExtension {
        public static string SpaceCamelCase(this string that)
        {

            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(that, " ");
        }

        public static string[] SplitCamelCase(this string source)
        {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}

namespace System.Linq
{
    public static class LinqExt
    {
        public static List<E> Select<T,E>(this IEnumerable<T> list, Func<int, T, E> conversion)
        {
            List<E> ret = new List<E>();
            int i = 0;
            foreach(var s in list)
            {
                ret.Add(conversion(i, s));
            }
            return ret;
        }
        public static List<E> Select<T, E>(this IList<T> list, Func<int, T, E> conversion)
        {
            List<E> ret = new List<E>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                ret.Add(conversion(i, list[i]));
            }
            return ret;
        }

        public static E[] Select<T, E>(this T[] list, Func<int, T, E> conversion)
        {
            E[] ret = new E[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                ret[i] = (conversion(i, list[i]));
            }
            return ret;
        }

        public static List<T> SubList<T>(this IList<T> list, int start, int length = -1)
        {
            if (length == -1) length = list.Count - start;

            List<T> ret = new List<T>(length);
            for (int i = 0; i < length; i++)
            {
                ret.Add(list[start+i]);
            }
            return ret;
        }

        public static T[] SubArray<T>(this T[] list, int start, int length = -1)
        {
            if (length == -1) length = list.Length - start;

            var ret = new T[length];
            for (int i = 0; i < length; i++)
            {
                ret[i] = (list[start + i]);
            }
            return ret;
        }

        public static void SwapElements<T>(this IList<T> list, int startIndex = 0)
        {
            //Debug.Log("Pre-Swap " + startIndex + " : " + list.ToCommaString());
            var swapMaxCount = startIndex + (list.Count - startIndex) / 2;

            for (int i = startIndex, count = 0; i < swapMaxCount; i++, ++count)
            {
                T temp = list[i];
                list[i] = list[list.Count - 1- count];
                list[list.Count - 1 - count] = temp;
            }
            //Debug.Log("Post-Swap " + list.ToCommaString());
        }
    }
}

namespace System
{
    public static class MathfExt
    {
        public static float Fraction(this float floatNumber)
        {
            return (float)(floatNumber - System.Math.Truncate(floatNumber));
        }

        
    }
}

namespace System.Collections.Generic {
    public static class SystemCollectionsGenericExtension
    {

        public static void AddRange<T,E>(this IList<T> list, IEnumerable<E> other) where T : class
        {
            foreach(var toAdd in other)
            {
                list.Add(toAdd as T);
            }
        }

        public static int IncreamentOrCreate<T>(this IDictionary<T, int> that, T newKey)
        {
            int value = 0;
            //Debug.Log(that + " search for " + (newKey == null ? "NULL" : newKey.ToString()) );
            if(that.TryGetValue(newKey, out value))
            {
                ++value;
                that[newKey] = value;
                return value;
            }

            that[newKey] = 1;
            return 1;
        }
        public static int Decreament<T>(this IDictionary<T, int> that, T newKey)
        {
            int value = 0;
            if(that.TryGetValue(newKey, out value))
            {
                --value;
                that[newKey] = value;
                return value;
            }
            that[newKey] = -1;
            return -1;

        }

        public static V GetOrInit<K,V>(this IDictionary<K,V> that, K key) where V : new()
        {
            V val = default(V);
            if(that.TryGetValue(key, out val))
            {
                return val;
            }
            val = new V();
            that[key] = val;
            return val;
        }
       
        public static void AddRange<T>(this HashSet<T> that, IEnumerable<T> toAdd)
        {
            foreach(var newAdded in toAdd)
            {
                that.Add(newAdded);
            }
        }

        public static void Foreach<T>(this IEnumerable<T> that, Action<T> func)
        {
            foreach (var t in that)
            {
                func(t);
            }
        }
    }
}
