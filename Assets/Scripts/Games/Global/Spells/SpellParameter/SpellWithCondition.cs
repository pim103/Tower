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
        public InstructionTargeting instructionTargeting { get; set; }
        public ConditionType conditionType { get; set; }

        public Effect effect { get; set; }
        public SpellComponent spellComponent { get; set; }

        /* list condition */
        /* Following effect need one of these condition */
        public Effect conditionEffect { get; set; }
        public int level { get; set; }
        public string nameSpellWithCondition { get; set; }
    }
}
