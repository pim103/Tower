using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
//using Games.Global.Patterns;
using Games.Global.Weapons;
using Games.Players;
using TMPro;
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
        private const int DEFAULT_ATT = 0;
        private const float DEFAULT_SPEED = 10;
        private const float DEFAULT_ATT_SPEED = 1;
        private const float DEFAULT_RESSOURCE = 50;
        
        private const int DEFAULT_NB_WEAPONS = 1;

        public float initialHp;
        public int initialDef;
        public int initialMagicalDef;
        public int initialPhysicalDef;
        public int initialAtt;
        public float initialSpeed;
        public float initialAttSpeed;
        public float initialRessource1;
        public float initialRessource2;

        public float hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public int att = DEFAULT_ATT;
        public float speed = DEFAULT_SPEED;
        public float attSpeed = DEFAULT_ATT_SPEED;
        public int magicalDef = 0;
        public int physicalDef = 0;

        public float ressource1 = DEFAULT_RESSOURCE;
        public float ressource2 = 0;

        public Func<AbilityParameters, bool> OnDamageReceive;

        public Func<AbilityParameters, bool> OnDamageDealt;

        public List<int> playerInBack;
        
        // If needed, create WeaponExposer to get all scripts of a weapon
        public List<Weapon> weapons;
        public List<Armor> armors;

        public TypeEntity typeEntity;

        // Suffered effect 
        public Dictionary<TypeEffect, Effect> underEffects;

        // Effect add to damage deal
        public List<Effect> damageDealExtraEffect;

        // Effect add to damage receive
        public List<Effect> damageReceiveExtraEffect;

        public List<BuffSpell> currentBuff;

        public List<Entity> entityInRange;
        
        public EntityPrefab entityPrefab;

        public bool doingSkill = false;

        public abstract void BasicAttack();
        public abstract void BasicDefense();
        public abstract void DesactiveBasicDefense();

        // Bool set by effect
        public bool isWeak = false;
        public bool canPierce = false;
        public bool isInvisible = false;
        public bool isUntargeatable = false;
        public bool isSleep = false;
        public bool canPierceOnBack = false;
        public bool hasThorn = false;
        public bool hasMirror = false;
        public bool isIntangible = false;
        public bool hasAntiSpell = false;
        public bool hasDivineShield = false;
        public bool shooldResurrect = false;
        public bool isSilence = false;
        public bool isConfuse = false;
        public bool hasWill = false;
        public bool isFeared = false;
        public bool isCharmed = false;
        public bool isBlind = false;
        public bool canBasicAttack = true;
        public bool hasLifeSteal = false;
        public bool hasTaunt = false;
        public bool hasNoAggro = false;
        public bool isUnkillableByBleeding = false;
        public bool isLinked = false;
        public bool hasRedirection = false;
        
        public void InitEquipementArray(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new Dictionary<TypeEffect, Effect>();
            damageDealExtraEffect = new List<Effect>();
            damageReceiveExtraEffect = new List<Effect>();
            entityInRange = new List<Entity>();
        }

        // Take true damage is usefull with effect pierce
        public virtual void TakeDamage(float initialDamage, AbilityParameters abilityParameters, bool takeTrueDamage)
        {
            float damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;

            Entity originDamage = abilityParameters.origin;
            
            // TODO : set var with correct bool
            bool isMagic = false;
            bool isPhysic = false;

            if (hasDivineShield || (isIntangible && isPhysic) || (hasAntiSpell && isMagic) || originDamage.isBlind)
            {
                return;
            }

            if (takeTrueDamage ||
                (originDamage.canPierceOnBack && 
                 playerInBack.Contains(abilityParameters.origin.IdEntity)
                ))
            {
                damageReceived = initialDamage;

                if (isMagic)
                {
                    damageReceived = (damageReceived - magicalDef) > 0 ? (damageReceived - magicalDef) : 0;
                } 
                else if (isPhysic)
                {
                    damageReceived = (damageReceived - physicalDef) > 0 ? (damageReceived - physicalDef) : 0;
                }
            }

            if (originDamage.hasLifeSteal)
            {
                originDamage.hp += damageReceived * originDamage.underEffects[TypeEffect.LifeSteal].level;
                if (originDamage.hp > originDamage.initialHp)
                {
                    originDamage.hp = originDamage.initialHp;
                }
            }

            if (hasRedirection && DataObject.invocationsInScene.Count > 0)
            {
                DataObject.invocationsInScene[0].ApplyDamage(damageReceived * 0.75f);
                ApplyDamage(damageReceived * 0.25f);
            }
            else
            {
                ApplyDamage(damageReceived);
            }

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

            List<Effect> effects = damageReceiveExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect).ToList();
            foreach (Effect effect in effects)
            {
                EffectController.ApplyEffect(this, effect);
            }

            if (isSleep)
            {
                EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Sleep]);
            }

            if (hasMirror && isMagic)
            {
                AbilityParameters newAbility = new AbilityParameters { origin = this };
                abilityParameters.origin.TakeDamage(initialDamage * 0.25f, newAbility, canPierce);
            }

            if (hasThorn && isPhysic)
            {
                AbilityParameters newAbility = new AbilityParameters { origin = this };
                abilityParameters.origin.TakeDamage(initialDamage * 0.25f, newAbility, canPierce);
            }
        }

        public virtual void ApplyDamage(float directDamage)
        {
            hp -= directDamage;
        }
    }
}