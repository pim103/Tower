using System;
using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public enum DistanceNeeded
    {
        Melee,
        Distance
    }

    public enum Target
    {
        Player,
        TargetLocked,
        AwayFromPlayer
    }
    
    public class MoveToTarget : Leaf
    {
        private readonly DistanceNeeded distanceNeeded;
        private readonly Target target;

        // Move to target, if bool is false, target is the player
        public MoveToTarget(DistanceNeeded distanceNeeded, Target target)
        {
            this.distanceNeeded = distanceNeeded;
            this.target = target;
        }
        
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;

            return target == Target.AwayFromPlayer ? 
                MoveAwayFromTheTarget(monster) : 
                GettingCloserToTheTarget(monster);
        }

        private TreeStatus MoveAwayFromTheTarget(Monster monster)
        {
            EntityPrefab entityTarget = DataObject.playerInScene.First().Value;
            float distanceFromPlayer = Vector3.Distance(monster.entityPrefab.transform.position, entityTarget.transform.position);

            if (distanceFromPlayer < 20.0f)
            {
                Vector3 dest = (entityTarget.transform.position - monster.entityPrefab.transform.position).normalized;

                if (monster.entityPrefab.navMeshAgent.SetDestination(dest))
                {
                    monster.entityPrefab.animator.SetFloat("Locomotion",1.0f);
                    return TreeStatus.RUNNING;
                }

                return TreeStatus.FAILURE;
            }

            return TreeStatus.SUCCESS;
        }

        private TreeStatus GettingCloserToTheTarget(Monster monster)
        {
            EntityPrefab entityTarget = target == Target.TargetLocked
                ? monster.entityPrefab.target.entityPrefab
                : DataObject.playerInScene.First().Value;

            float distanceFromPlayer = Vector3.Distance(monster.entityPrefab.transform.position, entityTarget.transform.position);
            if (distanceNeeded == DistanceNeeded.Distance)
            {
                if (distanceFromPlayer > 20.0f)
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(entityTarget.transform.position))
                    {
                        monster.entityPrefab.animator.SetFloat("Locomotion",1.0f);
                        return TreeStatus.RUNNING;
                    }
                }
                else
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(monster.entityPrefab.transform.position))
                    {
                        monster.entityPrefab.animator.SetFloat("Locomotion",0.0f);
                        return TreeStatus.SUCCESS;
                    }
                }
            }
            else
            {
                if (distanceFromPlayer > 2.0f)
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(entityTarget.transform.position))
                    {
                        monster.entityPrefab.animator.SetFloat("Locomotion",1.0f);
                        return TreeStatus.RUNNING;
                    }
                }
                else
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(monster.entityPrefab.transform.position))
                    {
                        monster.entityPrefab.animator.SetFloat("Locomotion",0.0f);
                        return TreeStatus.SUCCESS;
                    }
                }
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}
