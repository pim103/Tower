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
        public int id { get; set; } = 0;
        public Classes classes { get; set; }
        public CategoryWeapon categoryWeapon { get; set; }
        public Spell spell1 { get; set; }
        public Spell spell2 { get; set; }
        public Spell spell3 { get; set; }
    }
    
    public class ClassesList
    {
        public List<Classes> classes;
        public List<ClassesWeaponSpell> classesWeaponSpell;

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

        public List<ClassesWeaponSpell> GetSpellCategoryForClasses(Classes classes)
        {
            return classesWeaponSpell?.FindAll(data => data.classes.id == classes.id);
        }
        
        public List<Spell> GetSpellForClassesAndCategory(CategoryWeapon categoryWeapon, Classes classes)
        {
            List<Spell> spells = new List<Spell>();

            ClassesWeaponSpell classesWeaponSpell = this.classesWeaponSpell?.Find(data =>
                data.classes.id == classes.id && data.categoryWeapon.id == categoryWeapon.id);

            if (classesWeaponSpell != null)
            {
                spells.Clear();
                if (classesWeaponSpell.spell1 != null) spells.Add(classesWeaponSpell.spell1);
                if (classesWeaponSpell.spell2 != null) spells.Add(classesWeaponSpell.spell2);
                if (classesWeaponSpell.spell3 != null) spells.Add(classesWeaponSpell.spell3);
            }
            
            return spells;
        }

        public void InitClassesCategorySpells(string json)
        {
            classesWeaponSpell = new List<ClassesWeaponSpell>();
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                ClassesWeaponSpellJsonList classesListJson = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref classesListJson);

                foreach (ClassesWeaponSpellJson classesCategorySpell in classesListJson.classesCategory)
                {
                    classesWeaponSpell.Add(classesCategorySpell.Convert());
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error");
                Debug.Log(json);
                Debug.Log(e.Message);
            }
        }
    }
}