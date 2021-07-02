using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.CompositeBehavior;
using Games.Global.TreeBehavior.DecoratorBehavior;
using Games.Global.TreeBehavior.LeafBehavior;

namespace Games.Global.TreeBehavior.TestTreeBehavior
{
    public class GameContext : BehaviorStatus
    {
        public Monster CurrentMonster;
    }
    
    public class MonsterBasicBehavior
    {
        private TreeNode treeBehavior;
        private GameContext gc;
        
        public void InitBehaviorTree(Monster entity)
        {
            gc = new GameContext
            {
                CurrentMonster = entity
            };

            treeBehavior = CreateBehavior();
        }

        public void UpdateBehaviorTree()
        {
            //gc.CurrentMonster.ApplyDamage(0.1f);
            if (gc.CurrentMonster == null)
            {
                return;
            }
            
            treeBehavior.Execute(gc);
        }

        private TreeNode CreateBehavior()
        {
            DistanceNeeded distanceNeeded = (gc.CurrentMonster.monsterType == MonsterType.Distance ||
                                             gc.CurrentMonster.monsterType == MonsterType.Support) ? 
                DistanceNeeded.Distance :
                DistanceNeeded.Melee;
            
            // Heal - Protection
            Sequence checkOwnHp = new Sequence(
                new CheckHPPercentage(0.5f,true),
                new CheckSpellWithTag(SpellTag.HealHimself), 
                new LaunchSpellWithTag(SpellTag.HealHimself));

            Sequence checkDefensiveBuff = new Sequence(
                new CheckHPPercentage(0.5f,true),
                new CheckSpellWithTag(SpellTag.DefensiveBuff), 
                new LaunchSpellWithTag(SpellTag.DefensiveBuff));

            // RUUUUUUN
            Sequence runAwayPlayer = new Sequence(new CheckHPPercentage(0.1f, true),
                new ParallelNode(new MoveToTarget(DistanceNeeded.Distance, Target.AwayFromPlayer),
                    new LaunchBasicAttack()));

            Sequence useHealOnTarget = new Sequence(new CheckSight(true),
                new LaunchSpellWithTag(SpellTag.HealOther, SpellTag.HealArea));
            Selector useHealOrMove = new Selector(useHealOnTarget, new MoveToTarget(distanceNeeded, Target.TargetLocked));
            
            Sequence checkAlliesHp = new Sequence(
                new CheckHPPercentage(0.5f,false),
                new CheckSpellWithTag(SpellTag.HealOther, SpellTag.HealArea), 
                useHealOrMove);

            // Dispell
            Sequence alliesUnderDebuff = new Sequence(
                new AllyIsDebuff(), 
                new CheckSpellWithTag(SpellTag.DispelOther), 
                new LaunchSpellWithTag(SpellTag.DispelOther));

            Sequence underDebuffNotIncapaciting = new Sequence(
                new IsDebuff(), 
                new CheckSpellWithTag(SpellTag.DispelHimself), 
                new LaunchSpellWithTag(SpellTag.DispelHimself));

            Sequence enemyUnderBuff = new Sequence(
                new EnemyIsBuff(), 
                new CheckSpellWithTag(SpellTag.DispelEnemy), 
                new LaunchSpellWithTag(SpellTag.DispelEnemy));

            // Avoid skill
            Sequence useDash = new Sequence(new CheckSpellWithTag(SpellTag.Movement),
                new LaunchSpellWithTag(SpellTag.Movement));

            Selector avoidSkillSelector = new Selector(useDash, checkDefensiveBuff, new ExitAOE());

            Sequence underArea = new Sequence(new MonsterInAOE(), avoidSkillSelector);
            // TODO : add sequence to avoid proj

            // Use buff
            Sequence buffAvailable = new Sequence(new CheckSpellWithTag(SpellTag.OffensiveBuff), new LaunchSpellWithTag(SpellTag.OffensiveBuff));

            // Use spell
            Sequence useDistanceSpell = new Sequence(
                new CheckSight(),
                new CheckEnemyInRange(DistanceNeeded.Distance),
                new LaunchSpellWithTag(SpellTag.DistanceDamage, SpellTag.DistanceControl));

            Selector farAwayDistance = new Selector(useDistanceSpell, new MoveToTarget(DistanceNeeded.Distance, Target.Player));

            Sequence useDistanceSpellWhenPossible = new Sequence( 
                new CheckSpellWithTag(SpellTag.DistanceDamage, SpellTag.DistanceControl),
                farAwayDistance);

            Sequence useMeleeSpell = new Sequence(new CheckSight(),
                new CheckEnemyInRange(DistanceNeeded.Melee), 
                new LaunchSpellWithTag(SpellTag.MeleeDamage, SpellTag.MeleeControl));

            Selector farAwayMelee = new Selector(useMeleeSpell, new MoveToTarget(DistanceNeeded.Melee, Target.Player));

            Sequence useMeleeSpellWhenPossible = new Sequence( 
                new CheckSpellWithTag(SpellTag.MeleeDamage, SpellTag.MeleeControl),
                farAwayMelee);

            // Use basic attack
            Sequence useBasicAttackWhenPossible = new Sequence(
                new CheckSight(), 
                new CheckEnemyInRange(distanceNeeded), new LaunchBasicAttack());

            Selector useBasicAttackOrMove =
                new Selector(useBasicAttackWhenPossible, new MoveToTarget(distanceNeeded, Target.Player)); 
            
            AlwaysTrue useBasicAttack = new AlwaysTrue(useBasicAttackOrMove);

            // Behavior Tree
            Selector monsterBasicBehaviorSelector = new Selector(
                checkOwnHp, checkDefensiveBuff, 
                runAwayPlayer, checkAlliesHp, alliesUnderDebuff, 
                underDebuffNotIncapaciting, enemyUnderBuff, 
                underArea, buffAvailable, useDistanceSpellWhenPossible, 
                useMeleeSpellWhenPossible, useBasicAttack);

            return monsterBasicBehaviorSelector;
        }
    }
}