using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss
{
    public static class LinqExtension
    {
        public static int IndexOf<T>(this T[] that, System.Func<T, bool> search)
        {
            for(int i=0; i < that.Length; ++i)
            {
                if (search(that[i])) return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this IList<T> that, System.Func<T, bool> search)
        {
            for (int i = 0; i < that.Count; ++i)
            {
                if (search(that[i])) return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this T[] that, T search)
        {
            for (int i = 0; i < that.Length; ++i)
            {
                if (that[i].Equals(search)) return i;
            }
            return -1;
        }
    }
}