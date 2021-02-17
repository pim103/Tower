using System;
using System.Collections.Generic;
using FullSerializer;
using Games.Global;
using UnityEngine;
using Utils;

namespace Games.Players
{
    public class ClassesList
    {
        public List<Classes> classes;

        public ClassesList(string jsonObject)
        {
            classes = new List<Classes>();

            InitClassesFromJson(jsonObject);
        }

        private void InitClassesFromJson(string classesJson)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                ClassesListJsonObject classesListJson = null;
                data = fsJsonParser.Parse(classesJson);
                serializer.TryDeserialize(data, ref classesListJson);

                foreach (ClassesJsonObject classesJsonObject in classesListJson.classes)
                {
                    classes.Add(classesJsonObject.ConvertToClasses());
                }
                
                DictionaryManager.hasClassesLoad = true;
            }
            catch (Exception e)
            {
                Debug.Log("Error");
                Debug.Log(classesJson);
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }
    }
}