using System.Collections.Generic;
using System.Diagnostics;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Players;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Games.Global.Weapons
{
    public class Bow: Weapon
    {
        public Bow()
        {
           animationToPlay = "BowAttack";
           spellOfBasicAttack = "BowBasicAttack";

           warriorSpells = new List<string>();
           warriorSpells.Add("WarriorBow1");
           warriorSpells.Add("WarriorBow2");
           warriorSpells.Add("WarriorBow3");

           mageSpells = new List<string>();
           mageSpells.Add("MageBow1");
           mageSpells.Add("MageBow2");
           mageSpells.Add("MageBow3");

           rangerSpells = new List<string>();
           rangerSpells.Add("RangerBow1");
           rangerSpells.Add("RangerBow2");
           rangerSpells.Add("RangerBow3");

           rogueSpells = new List<string>();
           rogueSpells.Add("RogueBow1");
           rogueSpells.Add("RogueBow2");
           rogueSpells.Add("RogueBow3");
        }

        public override void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            if (isFirstIteration)
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.up * 90);
            }
            else
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.down * 90);
            }
        }
    }
}
