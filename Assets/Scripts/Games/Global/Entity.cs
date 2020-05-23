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
using Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utils;
using Debug = UnityEngine.Debug;

namespace Games.Global
{
    public enum TypeEntity
    {
        ALLIES,
        MOB
    }
    
    public enum BehaviorType
    {
        Player,
        MoveOnTargetAndDie,
        Distance,
        Melee,
        WithoutTarget
    }
    
    public enum AttackBehaviorType {
        Random,
        AllSpellsIFirst
    }

    // Class for mobs and players
    public class Entity: ItemModel
    {
        public int IdEntity;

        public bool isPlayer = false;
        public bool isSummon = false;
        
        private ItemModel itemModel;
        
        private const float DEFAULT_HP = 100;
        private const int DEFAULT_DEF = 10;
        private const float DEFAULT_ATT = 0;
        private const float DEFAULT_SPEED = 10;
        private const float DEFAULT_ATT_SPEED = 1;
        private const float DEFAULT_RESSOURCE = 50;
        
        private const int DEFAULT_NB_WEAPONS = 1;

        public float initialHp = DEFAULT_HP;
        public int initialDef;
        public int initialMagicalDef;
        public int initialPhysicalDef;
        public float initialAtt;
        public float initialSpeed;
        public float initialAttSpeed;
        public float initialRessource1;
        public float initialRessource2;

        public float hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public float att = DEFAULT_ATT;
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

        public Spell basicAttack;
        public Spell basicDefense;
        public List<Spell> spells;

        public BehaviorType BehaviorType;
        public AttackBehaviorType AttackBehaviorType;

        public bool doingSkill = false;

        public virtual void BasicAttack()
        {
            
        }

        public virtual void BasicDefense()
        {
            
        }

        public virtual void DesactiveBasicDefense()
        {
            
        }

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
        public bool hasPassiveDeactivate = false;
        public bool canRecast = false;
        public bool hasLifeLink = false;
        
        public void InitEntityList(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new List<Weapon>();
            armors = new List<Armor>();
            underEffects = new Dictionary<TypeEffect, Effect>();
            damageDealExtraEffect = new List<Effect>();
            damageReceiveExtraEffect = new List<Effect>();
            entityInRange = new List<Entity>();
            currentBuff = new List<BuffSpell>();
            spells = new List<Spell>();
        }

        // Take true damage is usefull with effect pierce
        public virtual void TakeDamage(float initialDamage, AbilityParameters abilityParameters, DamageType damageType, bool takeTrueDamage = false)
        {
            float damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;

            Entity originDamage = abilityParameters.origin;

            bool isMagic = damageType == DamageType.Magical;
            bool isPhysic = damageType == DamageType.Physical;

            if (hasDivineShield || (isIntangible && isPhysic) || (hasAntiSpell && isMagic) || originDamage.isBlind)
            {
                initialDamage = 0;
                damageReceived = 0;
            }
            
            if (isMagic)
            {
                damageReceived = (damageReceived - magicalDef) > 0 ? (damageReceived - magicalDef) : 0;
            } 
            else if (isPhysic)
            {
                damageReceived = (damageReceived - physicalDef) > 0 ? (damageReceived - physicalDef) : 0;
            }

            if (takeTrueDamage ||
                (originDamage.canPierceOnBack && 
                 playerInBack.Contains(abilityParameters.origin.IdEntity)
                ))
            {
                damageReceived = initialDamage;
            }

            if (originDamage.hasLifeLink)
            {
                if (originDamage.isSummon)
                {
                    GenericSummonSpell genericSummonSpell = (GenericSummonSpell) originDamage.entityPrefab;
                    genericSummonSpell.summoner.hp =
                        genericSummonSpell.summoner.hp + (damageReceived * 0.25f) <=
                        genericSummonSpell.summoner.initialHp
                            ? genericSummonSpell.summoner.hp + (damageReceived * 0.25f)
                            : genericSummonSpell.summoner.initialHp;
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
                EffectController.ApplyEffect(this, effect, originDamage, originDamage.entityPrefab.transform.position);
            }

            if (isSleep)
            {
                EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Sleep]);
            }

            BuffController.EntityReceivedDamage(this, originDamage);

            if (hasMirror && isMagic)
            {
                AbilityParameters newAbility = new AbilityParameters { origin = this };
                abilityParameters.origin.TakeDamage(initialDamage * 0.25f, newAbility, DamageType.Magical, canPierce);
            }

            if (hasThorn && isPhysic)
            {
                AbilityParameters newAbility = new AbilityParameters { origin = this };
                abilityParameters.origin.TakeDamage(initialDamage * 0.25f, newAbility, DamageType.Magical, canPierce);
            }
        }

        public virtual void ApplyDamage(float directDamage)
        {
            hp -= directDamage;
            
            if (hp <= 0)
            {
                if (shooldResurrect)
                {
                    hp = initialHp / 2;
                    EffectController.StopCurrentEffect(this, underEffects[TypeEffect.Resurrection]);

                    return;
                }

                entityPrefab.EntityDie();

                if (isPlayer)
                {
                    TowersWebSocket.TowerSender("OTHERS", GameController.staticRoomId, "Player", "SendDeath", null);
                    Debug.Log("Vous êtes mort");
                    Cursor.lockState = CursorLockMode.None;
                    SceneManager.LoadScene("MenuScene");
                }
            }
        }
    }
}