using System.Collections.Generic;
using Games.Global.Abilities;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Bow: Weapon
    {
        public Bow()
        {
           //pattern = //pattern[2];
           //pattern[0] = //pattern(PA_INST.BACK, 0.2f);
           //pattern[1] = //pattern(PA_INST.FRONT, 0.2f);
           animationToPlay = "BowAttack";
           idPoolProjectile = 0;
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

        }
    }
}
