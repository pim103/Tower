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

        public void PrintAttributes()
        {
            Debug.Log("Weapon " + modelName + " - " + equipmentName + " type : " + type + " Category : " + category + " cost " + cost);
            Debug.Log(" att : " + damage + " attSpd : " + attSpeed + " animation to play : " + animationToPlay);
//            Debug.Log(" onDamageReceive : " + OnDamageReceive + " OnDamageDeal : " + OnDamageDealt);
        }
    }
}