using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FullSerializer;
using PathCreation;
using UnityEngine;

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

        public static T Clone<T>(T origin) where T : class
        {
            // fsSerializer serializer = new fsSerializer();
            // fsData data;
            // fsData outData;
            //
            // serializer.TrySerialize(origin.GetType(), origin, out data);
            // string compressedJson = fsJsonPrinter.CompressedJson(data);
            //
            // T cloned = null;
            // outData = fsJsonParser.Parse(compressedJson);
            // serializer.TryDeserialize(outData, ref cloned);
            //
            // return cloned;
            return (T) DeepClone(origin);
        }

        private static object DeepClone(object origin)
        {
            Type t = origin.GetType();
            object clone = Activator.CreateInstance(t);

            PropertyInfo[] propertyInfos = t.GetProperties(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Type type = propertyInfo.PropertyType;
                var value = propertyInfo.GetValue(origin);

                if (value == null)
                {
                    propertyInfo.SetValue(clone, value);
                    continue;
                }

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type itemType = type.GetGenericArguments()[0];
                    IList targetList = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
                    
                    foreach (var itemList in (IEnumerable) value)
                    {
                        var cloneItemList = itemList.GetType().GetConstructor(Type.EmptyTypes) != null ? Clone(itemList) : itemList;

                        targetList.Add(cloneItemList);
                    }

                    propertyInfo.SetValue(clone, targetList);
                    continue;
                }

                if (type.IsArray)
                {
                    Array array = (Array)value;
                    Array targetArray = Array.CreateInstance(type, array.Length);
                    
                    for (int i = 0; i < array.Length; ++i)
                    {
                        object o = array.GetValue(i);

                        targetArray.SetValue(o.GetType().GetConstructor(Type.EmptyTypes) != null ? Clone(o) : o, i);
                    }

                    propertyInfo.SetValue(clone, targetArray);
                }
                else if (IsIgnoredType(type))
                {
                    propertyInfo.SetValue(clone, value);
                }
                else
                {
                    propertyInfo.SetValue(clone, Clone(value));
                }
            }

            return clone;
        }

        private static bool IsIgnoredType(Type type)
        {
            if (type.IsPrimitive || 
                type == typeof(Decimal) || 
                type == typeof(String) || 
                type == typeof(Texture2D) ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) ||
                type == typeof(BezierPath) ||
                type == typeof(Transform) ||
                type == typeof(GameObject) ||
                type == typeof(Vector3))
            {
                return true;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
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