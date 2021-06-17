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
    public enum TypeWeapon
    {
        Distance,
        Cac
    }

    public class Weapon : Equipement
    {
        public WeaponPrefab weaponPrefab;

        public int id { get; set; }
        public CategoryWeapon category { get; set; }
        public TypeWeapon type { get; set; }
        public int damage { get; set; }
        public float attSpeed { get; set; }

        public string animationToPlay { get; set; }

        public Spell basicAttack { get; set; }

        public void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
//                case Classes.Warrior:
//                    InitWeaponSpellWithJson(warriorSpells);
//                    break;
//                case Classes.Mage:
//                    InitWeaponSpellWithJson(mageSpells);
//                    break;
//                case Classes.Rogue:
//                    InitWeaponSpellWithJson(rogueSpells);
//                    break;
//                case Classes.Ranger:
//                    InitWeaponSpellWithJson(rangerSpells);
//                    break;
            }
        }

        public void InitWeapon()
        {
            InitBasicAttack();
        }

        private void InitBasicAttack()
        {
            Entity wielder = weaponPrefab.GetWielder();
            
            if (category?.spellAttack != null)
            {
                wielder.basicAttack = category.spellAttack;
            }
        }

        public void InitWeaponSpellWithJson(List<string> spellsToFind)
        {
            Entity wielder = weaponPrefab.GetWielder();

            if (spellsToFind == null)
            {
                return;
            }
            
            foreach (string spellString in spellsToFind)
            {
                Spell spell = DataObject.SpellList.GetSpellByName(spellString);

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
            Debug.Log("Weapon " + modelName + " - " + equipmentName + " type : " + type + " Category : " + category + " cost " + cost);
            Debug.Log(" att : " + damage + " attSpd : " + attSpeed + " animation to play : " + animationToPlay);
//            Debug.Log(" onDamageReceive : " + OnDamageReceive + " OnDamageDeal : " + OnDamageDealt);
        }
    }
}