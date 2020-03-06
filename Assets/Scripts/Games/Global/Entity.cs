using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Global
{
    public enum TypeEntity
    {
        PLAYER,
        MOB,
        BOSS
    }

    // Class for mobs and players
    public abstract class Entity : ItemModel
    {
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

        public List<Effect> underEffects;

        [FormerlySerializedAs("movementPattern")] public MovementPatternController movementPatternController;

        public abstract bool InitWeapon(int idWeapon);

        public abstract void BasicAttack();

        public abstract void BasicDefense();

        public IEnumerator EffectTime(Effect effect)
        {
            int index;
            Effect effectInList;
            if ((index = underEffects.FindIndex(currentEffect => currentEffect.typeEffect == effect.typeEffect)) != -1)
            {
                effectInList = underEffects[index];
                effectInList.level += 1;
                effectInList.durationInSeconds += effect.durationInSeconds;
                underEffects[index] = effectInList;
                
                yield break;
            }

            underEffects.Add(effect);

            index = underEffects.FindIndex(currentEffect => currentEffect.typeEffect == effect.typeEffect);
            effectInList = underEffects[index];

            while (effectInList.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.5f);
                effectInList.durationInSeconds -= 0.5f;
            }

            index = underEffects.FindIndex(currentEffect => currentEffect.typeEffect == effect.typeEffect);
            underEffects.RemoveAt(index);
        }

        public void InitEquipementArray(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new List<Effect>();
        }

        public virtual void TakeDamage(int initialDamage, AbilityParameters abilityParameters)
        {
            int damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;
            hp -= damageReceived;

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
        }

        public void ApplyEffect(TypeEffect typeEffect, int duration, int level)
        {
            Effect effect = new Effect();
            effect.level = level;
            effect.durationInSeconds = duration;
            effect.typeEffect = typeEffect;
            StartCoroutine(EffectTime(effect));
        }
    }
}