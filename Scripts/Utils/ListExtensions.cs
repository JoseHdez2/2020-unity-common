using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionMethods
{
    public static class ListExtensions
    {
        public static T Pop<T>(this List<T> list)
        {
            T val = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return val;
        }

        // https://stackoverflow.com/a/18867218/
        public static bool IsEmpty<T>(this IEnumerable<T> list) => !list.Any();

        // https://stackoverflow.com/a/3453301/
        public static List<T> TakeLast<T>(this List<T> list, int n) => list.Skip(Math.Max(0, list.Count() - n)).ToList();

        // https://stackoverflow.com/a/1547276
        public static T[] Concat<T>(this T[] a, T[] b)
        {
            var z = new T[a.Length + b.Length];
            a.CopyTo(z, 0);
            b.CopyTo(z, a.Length);
            return z;
        }
        
        public static T RandomItem<T>(this List<T> list) 
            => list[UnityEngine.Random.Range(0, list.Count - 1)];
    }
}