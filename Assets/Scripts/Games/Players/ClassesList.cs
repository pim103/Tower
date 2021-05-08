using System;
using System.Collections.Generic;
using System.Linq;
using FullSerializer;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace Games.Players
{
    public class ClassesWeaponSpell
    {
        public Classes classes;
        public CategoryWeapon categoryWeapon;
        public Spell spell1;
        public Spell spell2;
        public Spell spell3;
    }
    
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
            }
        }

        public Classes GetClassesFromId(int id)
        {
            return classes.Find(classe => classe.id == id);
        }

        public Classes GetFirstClasses()
        {
            return classes.First();
        }
    }
}