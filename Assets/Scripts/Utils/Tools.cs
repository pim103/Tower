using System;
using System.CodeDom;
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
                if (propertyInfo.GetType().IsArray)
                {
                    Array array = (Array)propertyInfo.GetValue(origin);
                    Array targetArray = Array.CreateInstance(propertyInfo.GetType(), array.Length);
                    
                    for (int i = 0; i < array.Length; ++i)
                    {
                        object o = array.GetValue(i);

                        if (o.GetType().GetConstructor(Type.EmptyTypes) != null)
                        {
                            targetArray.SetValue(Clone(o), i);
                        }
                        else
                        {
                            targetArray.SetValue(o, i);
                        }
                        
                    }

                    propertyInfo.SetValue(clone, targetArray);
                }
                else
                {
                    propertyInfo.SetValue(clone, propertyInfo.GetValue(origin));
                }
            }

            return clone;
        }

        private static bool IsEpsilon(float value)
        {
            return value >= -0.99999 && value <= 0.00001;
        }
        
        public static bool IsSimilar<T>(T origin, T compareObject)
        {
            PropertyInfo[] propertyInfosOriginalObj = origin.GetType().GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);

            bool isDifferent = false;

            foreach (PropertyInfo propertyInfo in propertyInfosOriginalObj)
            {
                if (propertyInfo.PropertyType == typeof(int) || propertyInfo.PropertyType.IsEnum)
                {
                    if ((int) propertyInfo.GetValue(origin) != (int) propertyInfo.GetValue(compareObject))
                    {
                        isDifferent = true;
                    }
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    if ((string) propertyInfo.GetValue(origin) != (string) propertyInfo.GetValue(compareObject))
                    {
                        isDifferent = true;
                    }
                }
                else if (propertyInfo.PropertyType == typeof(float))
                {
                    if (!IsEpsilon((float) propertyInfo.GetValue(origin) - (float) propertyInfo.GetValue(compareObject)))
                    {
                        isDifferent = true;
                    }
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    if ((bool) propertyInfo.GetValue(origin) != (bool) propertyInfo.GetValue(compareObject))
                    {
                        isDifferent = true;
                    }
                }
                else if (propertyInfo.GetValue(origin) != propertyInfo.GetValue(compareObject))
                {
                    isDifferent = true;
                }

                if (isDifferent)
                {
                    return false;
                }
            }

            return true;
        }
        
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width*height];
 
            for(int i = 0; i < pix.Length; i++)
                pix[i] = col;
 
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
 
            return result;
        }
    }
}