using System;
using System.Collections.Generic;
using Games.Global.Abilities.MobAbilities;
using Games.Global.Abilities.WeaponsAbilities;

namespace Games.Global.Abilities
{
    public enum AbilityDico
    {
        PLAYER,
        MOB,
        WEAPON
    }

    public struct AbilityParameters
    {
        public Entity origin;
        public Entity directTarget;

        public List<Entity> allies;
        public List<Entity> enemies;
    }

    public static class AbilityManager
    {
        private static Dictionary<string, Func<AbilityParameters, bool>> methodWeaponList;

        private static Dictionary<string, Func<AbilityParameters, bool>> methodMobList;

        private static void InitMobAbilities()
        {
            methodMobList = new Dictionary<string, Func<AbilityParameters, bool>>();
            
            // Add Demon Abilities
            methodMobList.Add("Aie", DemonSkill.Aie);
            methodMobList.Add("Outch", DemonSkill.Outch);
            methodMobList.Add("EjectTarget", DemonSkill.EjectTarget);
            methodMobList.Add("ThrowPoison", DemonSkill.ThrowPoison);
        }

        private static void InitWeaponAbilities()
        {
            methodWeaponList = new Dictionary<string, Func<AbilityParameters, bool>>();
            
            // Add short sword abilities
            methodWeaponList.Add("ApplyFire", ShortSwordAbiltiy.ApplyFire);
            methodWeaponList.Add("KillHim", ShortSwordAbiltiy.KillHim);
            
            // Add spear abilities
            methodWeaponList.Add("PierceHim", SpearAbility.PierceHim);
            methodWeaponList.Add("Explode", SpearAbility.Explode);
        }

        public static void InitAbilities()
        {
            InitMobAbilities();
            InitWeaponAbilities();
        }

        public static Func<AbilityParameters, bool> GetAbility(string methodName, AbilityDico dico)
        {
            switch (dico)
            {
                case AbilityDico.MOB:
                    return methodMobList[methodName];
                    break;
                case AbilityDico.PLAYER:
                    break;
                case AbilityDico.WEAPON:
                    return methodWeaponList[methodName];
                    break;
            }

            return null;
        }
    }
}
