using System;
using System.Collections.Generic;
using System.Reflection;
using Games.Global;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class Tools
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static T Clone<T>(T origin) where T: new()
        {
            T clone = new T();
            PropertyInfo[] propertyInfos = origin.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyInfo.SetValue(clone, propertyInfo.GetValue(origin));
            }

            return clone;
        }
    }
}