using System;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;

//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Spear : Weapon
    {
        public Spear()
        {
            animationToPlay = "SpearAttack";
            //pattern = //pattern[2];
            //pattern[0] = //pattern(PA_INST.FRONT, 1, 0.2f, 0.01f);
            //pattern[1] = //pattern(PA_INST.BACK, 1, 0.2f, 0.01f);
        }
        
        public override void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            if (isFirstIteration)
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.up * 20);
            }
            else
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.down * 20);
            }
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    break;
            }
        }
    }
}