using System;
using System.Collections.Generic;
using System.IO;
using FullSerializer;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    public enum CategoryWeapon
    {
        SHORT_SWORD,
        LONG_SWORD,
        SPEAR,
        AXE,
        TWO_HAND_AXE,
        HAMMER,
        HALBERD,
        MACE,
        BOW,
        STAFF,
        DAGGER,
        TRIDENT,
        RIFLE,
        CROSSBOW,
        SLING,
        HANDGUN
    };

    public enum TypeWeapon
    {
        Distance,
        Cac
    }

    public abstract class Weapon : Equipement
    {
        public WeaponPrefab weaponPrefab;
        
        public string equipementName;
        public CategoryWeapon category;
        public TypeWeapon type;
        public int damage;
        public float attSpeed;

        public string animationToPlay;
        public string spellOfBasicAttack;

        public List<string> warriorSpells;
        public List<string> mageSpells;
        public List<string> rogueSpells;
        public List<string> rangerSpells;

        public Spell basicAttack;

        public virtual void InitPlayerSkill(Classes classe)
        {
        }

        public virtual void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            
        }

        public void InitWeapon()
        {
            InitBasicAttack();
        }

        public void InitBasicAttack()
        {
            string path = Application.dataPath + "/Data/SpellsJson/" + spellOfBasicAttack + ".json";
            Spell spell = FindSpellWithPath(path);
            Entity wielder = weaponPrefab.GetWielder();

            if (spell != null)
            {
                wielder.basicAttack = spell;
            }
        }

        public void InitWeaponSpellWithJson(List<string> spellsToFind)
        {
            string path = Application.dataPath + "/Data/SpellsJson/";
            string tempPath = "";

            Entity wielder = weaponPrefab.GetWielder();
            Spell spell;

            foreach (string spellString in spellsToFind)
            {
                tempPath = path + spellString + ".json";

                spell = FindSpellWithPath(tempPath);

                if (spell == null)
                {
                    continue;
                }

                wielder.spells.Add(spell);
                if (weaponPrefab.GetWielder().isPlayer)
                {
                    PlayerPrefab playerPrefab = (PlayerPrefab) wielder.entityPrefab;
                    if (wielder.spells.Count == 1)
                    {
                        playerPrefab.spell1.text = spellString;
                    } else if (wielder.spells.Count == 2)
                    {
                        playerPrefab.spell1.text = spellString;
                    } else if (wielder.spells.Count == 3)
                    {
                        playerPrefab.spell1.text = spellString;
                    }
                }
            }
        }

        public Spell FindSpellWithPath(string tempPath)
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
                spell = SpellController.Clone(spell);
            }
            catch (Exception e)
            {
//                Debug.Log("Cant import spell for path : " + tempPath);
//                Debug.Log(e.Message);
            }

            return spell;
        }
    }
}