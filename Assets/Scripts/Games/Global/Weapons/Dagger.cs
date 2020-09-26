using System;
using System.Collections.Generic;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            animationToPlay = "doingDaggerAttack";
            spellOfBasicAttack = "DaggerBasicAttack";
            
            warriorSpells = new List<string>();
            warriorSpells.Add("WarriorDagger1");
            warriorSpells.Add("WarriorDagger2");
            warriorSpells.Add("WarriorDagger3");

            mageSpells = new List<string>();
            mageSpells.Add("MageDagger1");
            mageSpells.Add("MageDagger2");
            mageSpells.Add("MageDagger3");

            rangerSpells = new List<string>();
            rangerSpells.Add("RangerDagger1");
            rangerSpells.Add("RangerDagger2");
            rangerSpells.Add("RangerDagger3");

            rogueSpells = new List<string>();
            rogueSpells.Add("RogueDagger1");
            rogueSpells.Add("RogueDagger2");
            rogueSpells.Add("RogueDagger3");
        }
        
        public override void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            if (isFirstIteration)
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.up * 70);
            }
            else
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.down * 70);
            }
        }
    }
}