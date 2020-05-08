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
        
        IfTargetHasEffect,
        IfPlayerHasEffect,
        IfTargetDies,

        EnemiesInArea,
        DamageIfTargetHasEffect
    }
    
    public class SpellWithCondition
    {
        public InstructionTargeting instructionTargeting;
        public ConditionType conditionType;

        public Effect effectOnSelf;
        public SpellComponent spellComponent;

        /* list condition */
        /* Following effect need one of these condition */
        public Effect conditionEffect;
        public int level;
    }
}
