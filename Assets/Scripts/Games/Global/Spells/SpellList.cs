using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using FullSerializer;
using Games.Global.Armors;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Networking;
using TestC;
using UnityEngine;
using Utils;

namespace Games.Global.Spells
{
    public class SpellList
    {
        public class SpellInfo
        {
            public string filename;
            public int id;
            public string spellName;
            public Spell spell;
        }
        
        // NameSpell - Spell
        public List<SpellInfo> SpellInfos;

        public SpellList(string json)
        {
            SpellInfos = new List<SpellInfo>();
            
            InitSpellDictionnary(json);
        }

        public Spell GetSpellByName(string name)
        {
            SpellInfo spellInfo;
            if (String.IsNullOrEmpty(name) || (spellInfo = SpellInfos.Find(info => info.spellName == name)) == null)
            {
                return null;
            }

            return Tools.Clone(spellInfo.spell);
        }
        
        private void InitSpellDictionnary(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                SpellListObject spellList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref spellList);

                foreach (SpellJsonObject spellJsonObject in spellList.skills)
                {
                    DownloadSpell(spellJsonObject.name);
                    Spell currentSpell = LoadSpellByName(spellJsonObject.name);

                    if (currentSpell == null)
                    {
                        Debug.Log("spell not found");
                        continue;
                    }

                    int id = Int32.Parse(spellJsonObject.id);
                    currentSpell.id = id;

                    SpellInfo spellInfo = new SpellInfo
                    {
                        filename = spellJsonObject.name,
                        id = id,
                        spellName = currentSpell.nameSpell,
                        spell = currentSpell
                    };
                    
                    SpellInfos.Add(spellInfo);
                }

                DictionaryManager.hasSpellsLoad = true;
            }
            catch (Exception e)
            {
                Debug.Log("Error");
                Debug.Log(json);
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }
        
        private static Spell LoadSpellByName(string filenameSpell)
        {
            string path = "Assets/Data/SpellsJson/" + filenameSpell + ".json";
            Spell spell = FindSpellWithPath(path);

            return spell;
        }
        
        private static Spell FindSpellWithPath(string tempPath)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;
            Spell spell = null;
            string jsonSpell;

            try
            {
                jsonSpell = File.ReadAllText(tempPath);
                data = fsJsonParser.Parse(jsonSpell);
                serializer.TryDeserialize(data, ref spell);
                spell = Tools.Clone(spell);
            }
            catch (Exception e)
            {
                Debug.Log("Cant import spell for path : " + tempPath);
                Debug.Log(e.Message);
            }

            return spell;
        }

        private void DownloadSpell(string filename)
        {
            using var client = new WebClient();

            client.DownloadFile(new Uri($@"{NetworkingController.PublicURL}/data/spell/{filename}.json"), 
                Application.dataPath + "/Data/SpellsJson/" + filename + ".json");
        }
    }
}