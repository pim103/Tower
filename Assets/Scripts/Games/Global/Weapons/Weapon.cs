using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FullSerializer;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Players;
//using UnityEditor.Animations;
using UnityEngine;
using Utils;

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

    public class Weapon : Equipement
    {
        public WeaponPrefab weaponPrefab;
        
        public int id { get; set; }
        public string equipementName { get; set; }
        public CategoryWeapon category { get; set; }
        public TypeWeapon type { get; set; }
        public int damage { get; set; }
        public float attSpeed { get; set; }

        public string animationToPlay { get; set; }
        public string spellOfBasicAttack { get; set; }

        public List<string> warriorSpells { get; set; }
        public List<string> mageSpells { get; set; }
        public List<string> rogueSpells { get; set; }
        public List<string> rangerSpells { get; set; }

        public Spell basicAttack { get; set; }

        public void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Warrior:
                    InitWeaponSpellWithJson(warriorSpells);
                    break;
                case Classes.Mage:
                    InitWeaponSpellWithJson(mageSpells);
                    break;
                case Classes.Rogue:
                    InitWeaponSpellWithJson(rogueSpells);
                    break;
                case Classes.Ranger:
                    InitWeaponSpellWithJson(rangerSpells);
                    break;
            }
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
            Spell spell = SpellController.LoadSpellByName(spellOfBasicAttack);
            Entity wielder = weaponPrefab.GetWielder();

            if (spell != null)
            {
                wielder.basicAttack = spell;
            }
        }

        public void InitWeaponSpellWithJson(List<string> spellsToFind)
        {
            Entity wielder = weaponPrefab.GetWielder();

            foreach (string spellString in spellsToFind)
            {
                Spell spell = SpellController.LoadSpellByName(spellString);

                if (spell == null)
                {
                    Debug.Log("Pas de spells");
                    continue;
                }

                wielder.spells.Add(spell);
                if (weaponPrefab.GetWielder().isPlayer)
                {
                    PlayerPrefab playerPrefab = (PlayerPrefab) wielder.entityPrefab;
                    if (wielder.spells.Count == 1)
                    {
                        playerPrefab.spell1.text = spell.nameSpell;
                    } else if (wielder.spells.Count == 2)
                    {
                        playerPrefab.spell2.text = spell.nameSpell;
                    } else if (wielder.spells.Count == 3)
                    {
                        playerPrefab.spell3.text = spell.nameSpell;
                    }
                }
            }
        }

        public void PrintAttributes()
        {
            Debug.Log("Weapon " + modelName + " - " + equipementName + " type : " + type + " Category : " + category + " cost " + cost);
            Debug.Log(" att : " + damage + " attSpd : " + attSpeed + " animation to play : " + animationToPlay);
            Debug.Log(" onDamageReceive : " + OnDamageReceive + " OnDamageDeal : " + OnDamageDealt);
        }
    }
}