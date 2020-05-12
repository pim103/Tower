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

           ProjectileSpell projectile = new ProjectileSpell
           {
               damages = 0,
               duration = 5,
               speed = 15,
               damageType = DamageType.Physical,
               isBasicAttack = true,
               idPoolObject = 0,
               OriginalPosition = OriginalPosition.Caster,
               OriginalDirection = OriginalDirection.Forward,
               needPositionToMidToEntity = true
           };

           basicAttack = new Spell
           {
               cost = 0,
               cooldown = 1,
               castTime = 0,
               activeSpellComponent = projectile
           };
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

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Warrior:
                    
                    break;
            }
        }
    }
}
