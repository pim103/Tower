using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Games.Global
{
    public enum TypeEntity
    {
        PLAYER,
        MOB,
        BOSS
    }

    // Class for mobs and players
    public abstract class Entity: ItemModel
    {
        private ItemModel itemModel;
        
        private const int DEFAULT_HP = 100;
        private const int DEFAULT_DEF = 10;
        private const int DEFAULT_ATT = 10;
        private const int DEFAULT_SPEED = 10;
        private const int DEFAULT_NB_WEAPONS = 1;

        public int initialHp;
        public int initialDef;
        public int initialAtt;
        public int initialSpeed;

        public int hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public int att = DEFAULT_ATT;
        public int speed = DEFAULT_SPEED;

        public Func<AbilityParameters, bool> OnDamageReceive;

        public Func<AbilityParameters, bool> OnDamageDealt;

        // If needed, create WeaponExposer to get all scripts of a weapon
        public List<Weapon> weapons;
        public List<Armor> armors;

        public TypeEntity typeEntity;

        public Dictionary<TypeEffect, Effect> underEffects;

        public EffectInterface effectInterface;

        public abstract void BasicDefense();

        public void ApplyEffect(TypeEffect typeEffect, int duration, int level)
        {
            Effect effect = new Effect();
            effect.level = level;
            effect.durationInSeconds = duration;
            effect.typeEffect = typeEffect;
            
            if (underEffects.ContainsKey(effect.typeEffect))
            {
                Effect effectInList = underEffects[effect.typeEffect];
                effectInList.UpdateEffect(effect);

                underEffects[effect.typeEffect] = effectInList;
                return;
            }

            effectInterface.StartCoroutineEffect(effect);
        }

        public void TriggerEffect(Effect effect)
        {
            switch (effect.typeEffect)
            {
                case TypeEffect.Burn:
                    if (underEffects.ContainsKey(TypeEffect.Sleep))
                    {
                        underEffects.Remove(TypeEffect.Sleep);
                    }

                    ApplyDamage(2);
                    break;
                case TypeEffect.Bleed:
                    ApplyDamage(1 * effect.level);
                    break;
                case TypeEffect.Poison:
                    ApplyDamage(1);
                    break;
            }
        }

        public void InitEquipementArray(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new Dictionary<TypeEffect, Effect>();
        }

        public virtual void TakeDamage(int initialDamage, AbilityParameters abilityParameters)
        {
            int damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;
            ApplyDamage(damageReceived);

            if (OnDamageReceive != null)
            {
                OnDamageReceive(abilityParameters);
            }

            foreach (Weapon weapon in weapons)
            {
                weapon.OnDamageReceive(abilityParameters);
            }

            foreach (Armor armor in armors)
            {
                armor.OnDamageReceive(abilityParameters);
            }

            if (underEffects.ContainsKey(TypeEffect.Sleep))
            {
                underEffects.Remove(TypeEffect.Sleep);
            }
        }

        public virtual void ApplyDamage(int directDamage)
        {
            hp -= directDamage;
        }
    }
}