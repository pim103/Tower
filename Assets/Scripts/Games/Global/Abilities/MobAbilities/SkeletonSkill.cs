using System;
using Random = UnityEngine.Random;

namespace Games.Global.Abilities.MobAbilities
{
    public class SkeletonSkill
    {
        public static bool Resurrect(AbilityParameters param)
        {
            if ((param.Self.hp - param.DamageDeal) <= 0)
            {
                int rand = Random.Range(0, 100);

                if (rand > 10)
                {
                    
                }
            }
            
            return true;
        }
    }
}