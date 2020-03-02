using System;
using System.Collections.Generic;
using System.IO;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Utils
{
    public static class ParserJson<T> where T : ObjectParsed, new()
    {
        public static List<T> Parse(StreamReader file, string header)
        {
            Stack<bool> openBracket = new Stack<bool>();
            
            List<T> jsonObjects = new List<T>();
            bool fileIsValid = false;

            string line = "";
            string key = "";
            string value = "";
            int indexComma;
            int indexColon;
            int indexEndOfArray;

            bool isInArray = false;
            
            T jsonObject = new T();

            while (!file.EndOfStream)
            {
                line = file.ReadLine();

                // Check if file contain header name "weapons"
                if (!fileIsValid && line != null && line.Contains(header))
                {
                    fileIsValid = true;
                    continue;
                }
                else if (!fileIsValid || line == null)
                {
                    continue;
                }

                indexComma = line.IndexOf(",");
                if (indexComma == -1)
                {
                    indexComma = line.Length;
                }
                
                if (!isInArray)
                {
                    if (line.Contains("{"))
                    {
                        openBracket.Push(true);
                    }
                    
                    if (line.Contains("}"))
                    {
                        openBracket.Pop();

                        if (openBracket.Count == 0)
                        {
                            jsonObjects.Add(jsonObject);
                            jsonObject = new T();   
                        }
                        else
                        {
                            jsonObject.DoSomething();
                        }
                    }

                    // If condition if verify, array closed is the array with key = "weapons" => EndOfFile
                    if (line.Contains("]") && !isInArray)
                    {
                        break;
                    }
                    
                    //Get information from file
                    indexColon = line.IndexOf(":");
                
                    if (indexColon == -1)
                    {
                        continue;
                    }

                    key = line.Substring(0, indexColon).Replace('"', ' ').Trim();
                    value = line.Substring(indexColon + 1, indexComma - indexColon - 1).Replace('"', ' ').Trim();

                    if (value.Contains("["))
                    {
                        isInArray = true;
                        value = value.Replace("[", " ").Trim();
                    }
                }
                else
                {
                    indexEndOfArray = line.IndexOf("]");
                    if (indexEndOfArray == -1)
                    {
                        indexEndOfArray = indexComma;
                    }
                    else
                    {
                        isInArray = false;
                    }
                    
                    value = line.Substring(0, indexEndOfArray).Replace('"', ' ').Trim();
                }

                if (value == "")
                {
                    continue;
                }

                jsonObject.InsertValue(key, value);
            }

            return jsonObjects;
        }
    }
}
