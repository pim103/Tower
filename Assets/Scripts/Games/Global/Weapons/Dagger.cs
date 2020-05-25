using System;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            animationToPlay = "DaggerAttack";
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

        public override void InitPlayerSkill(Classes classe)
        {
        }
    }
}