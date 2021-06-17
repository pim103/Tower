using System;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.CompositeBehavior;
using Games.Global.TreeBehavior.LeafBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.TestTreeBehavior
{
    public class GameContext : BehaviorStatus
    {
        public Monster CurrentMonster;
    }
    
    public class MonsterBasicBehavior : MonoBehaviour
    {
        private TreeNode treeBehavior;
        private GameContext gc;
        
        private void Start()
        {
            gc = new GameContext
            {
                CurrentMonster = GetComponent<MonsterPrefab>().GetMonster()
            };

            treeBehavior = CreateBehavior();
        }

        private void FixedUpdate()
        {
            gc.CurrentMonster.ApplyDamage(0.1f);
            treeBehavior.Execute(gc);
        }

        private TreeNode CreateBehavior()
        {
            Sequence seq = new Sequence(new CheckHPPercentage(0.7f,true), new CheckSpellWithTag(SpellTag.HealHimself), new LaunchSpellWithTag(SpellTag.HealHimself));

            Sequence selectAoe = new Sequence(seq);

            return selectAoe;
        }
    }
}