﻿using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckEnemyInRange : Leaf
    {
        private readonly DistanceNeeded distanceNeeded;

        public CheckEnemyInRange(DistanceNeeded distanceNeeded)
        {
            this.distanceNeeded = distanceNeeded;
        }
        
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            PlayerPrefab player = DataObject.playerInScene.First().Value;

            float distanceFromPlayer = Vector3.Distance(monster.entityPrefab.transform.position, player.transform.forward);
            float maxDistance = distanceNeeded == DistanceNeeded.Distance ? 30f : 5f;

            if (distanceFromPlayer < maxDistance)
            {
                return TreeStatus.SUCCESS;
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            
        }
    }
}