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
            treeBehavior.Execute(gc);
        }

        private TreeNode CreateBehavior()
        {
            //Sequence seq = new Sequence(new CheckHeal());
            Sequence secDefense = new Sequence(new CheckSpellWithTag(SpellTag.DefensiveBuff), new LaunchSpellWithTag(SpellTag.DefensiveBuff));
            Sequence secDash = new Sequence(new CheckSpellWithTag(SpellTag.Movement), new LaunchSpellWithTag(SpellTag.Movement));
            Selector selectExit = new Selector(secDash, secDefense, new ExitAOE());

            Sequence selectAoe = new Sequence(new MonsterInAOE(), selectExit);

            return selectAoe;
        }
    }
}