namespace Games.Global.Spells.SpellParameter
{
    public enum InstructionTargeting
    {
        ApplyOnSelf,
        DeleteOnSelf,
        ApplyOnTarget,
        DeleteOnTarget
    }
    
    public enum ConditionType
    {
        PlayerDies,
        PlayerDoesntDie,
        
        IfTargetHasEffectWhenHit,
        IfPlayerHasEffect,
        IfTargetDies,

        MinEnemiesInArea,
        DamageIfTargetHasEffect
    }
    
    public class SpellWithCondition
    {
        public InstructionTargeting instructionTargeting;
        public ConditionType conditionType;

        public Effect effect;
        public SpellComponent spellComponent;

        /* list condition */
        /* Following effect need one of these condition */
        public Effect conditionEffect;
        public int level;
    }
}
