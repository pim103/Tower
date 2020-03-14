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
using Utils;
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
        public int IdEntity;
        
        private ItemModel itemModel;
        
        private const float DEFAULT_HP = 100;
        private const int DEFAULT_DEF = 10;
        private const int DEFAULT_ATT = 10;
        private const int DEFAULT_SPEED = 10;
        private const float DEFAULT_ATT_SPEED = 1;
        private const float DEFAULT_RESSOURCE = 50;
        
        private const int DEFAULT_NB_WEAPONS = 1;

        public float initialHp;
        public int initialDef;
        public int initialAtt;
        public int initialSpeed;
        public float initialAttSpeed;
        public float initialRessource1;
        public float initialRessource2;

        public float hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public int att = DEFAULT_ATT;
        public int speed = DEFAULT_SPEED;
        public float attSpeed = DEFAULT_ATT_SPEED;

        public float ressource1 = DEFAULT_RESSOURCE;
        public float ressource2 = 0;

        public Func<AbilityParameters, bool> OnDamageReceive;

        public Func<AbilityParameters, bool> OnDamageDealt;


        // If needed, create WeaponExposer to get all scripts of a weapon
        public List<Weapon> weapons;
        public List<Armor> armors;

        public TypeEntity typeEntity;

        // Suffered effect 
        public Dictionary<TypeEffect, Effect> underEffects;

        // Effect add to damage deal
        public Dictionary<TypeEffect, Effect> damageDealExtraEffect;

        // Effect add to damage receive
        public Dictionary<TypeEffect, Effect> damageReceiveExtraEffect;

        public EffectInterface effectInterface;
        public EntityPrefab entityPrefab;

        public bool doingSkill = false;

        public abstract void BasicAttack();
        public abstract void BasicDefense();
        public abstract void DesactiveBasicDefense();

        public void ApplyNewEffect(TypeEffect typeEffect, float duration, int level = 1, Entity originEffect = null, float ressourceCost = 0)
        {
            Effect effect = new Effect();
            effect.level = level;
            effect.durationInSeconds = duration;
            effect.typeEffect = typeEffect;

            effect.launcher = originEffect;
            effect.ressourceCost = ressourceCost;

            if (ressource1 < ressourceCost)
            {
                return;
            }

            if (underEffects.ContainsKey(effect.typeEffect))
            {
                Effect effectInList = underEffects[effect.typeEffect];
                effectInList.UpdateEffect(effect);

                underEffects[effect.typeEffect] = effectInList;
                return;
            }

            effectInterface.StartCoroutineEffect(effect);
        }
        
        public void ApplyEffect(Effect effect)
        {
            if (underEffects.ContainsKey(effect.typeEffect))
            {
                Effect effectInList = underEffects[effect.typeEffect];
                effectInList.UpdateEffect(effect);

                underEffects[effect.typeEffect] = effectInList;
                return;
            }

            effectInterface.StartCoroutineEffect(effect);
        }

        public void RemoveEffect(TypeEffect typeEffect)
        {
            if (underEffects.ContainsKey(typeEffect))
            {
                effectInterface.StopCurrentEffect(underEffects[typeEffect]);
            }
        }

        public void InitialTrigger(Effect effect)
        {
            switch (effect.typeEffect)
            {
                case TypeEffect.Invisibility:
                    entityPrefab.SetMaterial(StaticMaterials.invisibleMaterial);
                    break;
                case TypeEffect.AttackSpeedUp:
                    attSpeed = initialAttSpeed + (0.5f * effect.level);
                    break;
            }
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

                    ApplyDamage(0.2f);
                    break;
                case TypeEffect.Bleed:
                    ApplyDamage(0.1f * effect.level);
                    break;
                case TypeEffect.Poison:
                    ApplyDamage(0.1f);
                    break;
                case TypeEffect.Regen:
                    hp += 0.2f;
                    break;
            }
        }

        public void EndEffect(Effect effect)
        {
            switch (effect.typeEffect)
            {
                case TypeEffect.Invisibility:
                    entityPrefab.SetMaterial(StaticMaterials.defaultMaterial);
                    break;
                case TypeEffect.AttackSpeedUp:
                    attSpeed = initialAttSpeed;
                    break;
            }
        }

        public void InitEquipementArray(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new Dictionary<TypeEffect, Effect>();
            damageDealExtraEffect = new Dictionary<TypeEffect, Effect>();
            damageReceiveExtraEffect = new Dictionary<TypeEffect, Effect>();
        }

        public virtual void TakeDamage(float initialDamage, AbilityParameters abilityParameters)
        {
            float damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;

            if (underEffects.ContainsKey(TypeEffect.BrokenDef))
            {
                damageReceived = initialDamage;
            }

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

            foreach (KeyValuePair<TypeEffect, Effect> effects in damageReceiveExtraEffect)
            {
                ApplyEffect(effects.Value);
            }

            if (underEffects.ContainsKey(TypeEffect.Sleep))
            {
                underEffects.Remove(TypeEffect.Sleep);
            }
        }

        public virtual void ApplyDamage(float directDamage)
        {
            hp -= directDamage;
        }
    }
}