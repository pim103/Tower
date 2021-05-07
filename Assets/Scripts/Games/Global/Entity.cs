using System.Collections.Generic;
using System.Linq;
using Games.Global.Armors;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Networking;
using Networking.Client;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Vector3 = System.Numerics.Vector3;

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
        public int IdEntity { get; set; }

        public bool isPlayer { get; set; } = false;
        public bool isSummon { get; set; } = false;

        private const float DEFAULT_HP  = 100;
        private const int DEFAULT_DEF = 10;
        private const float DEFAULT_ATT = 0;
        private const float DEFAULT_SPEED = 10;
        private const float DEFAULT_ATT_SPEED = 0;
        private const float DEFAULT_RESSOURCE = 50;
        
        private const int DEFAULT_NB_WEAPONS = 1;

        public float initialHp { get; set; } = DEFAULT_HP;
        public int initialDef { get; set; }
        public int initialMagicalDef { get; set; }
        public int initialPhysicalDef { get; set; }
        public float initialAtt { get; set; }
        public float initialSpeed { get; set; }
        public float initialAttSpeed { get; set; }
        public float initialRessource1 { get; set; }
        public float initialRessource2 { get; set; }

        public float hp { get; set; } = DEFAULT_HP;
        public int def { get; set; } = DEFAULT_DEF;
        public float att { get; set; } = DEFAULT_ATT;
        public float speed { get; set; } = DEFAULT_SPEED;
        public float attSpeed { get; set; } = DEFAULT_ATT_SPEED;
        public int magicalDef { get; set; } = 0;
        public int physicalDef { get; set; } = 0;

        public float ressource { get; set; } = DEFAULT_RESSOURCE;
        public float ressource2 { get; set; } = 0;

        public List<int> playerInBack { get; set; }
        
        // If needed, create WeaponExposer to get all scripts of a weapon
        public Weapon weapon { get; set; }
        public List<Armor> armors { get; set; }

        private TypeEntity typeEntity { get; set; }

        // Suffered effect 
        private List<Effect> underEffects;

        // Effect add to damage deal
        public List<Effect> damageDealExtraEffect { get; set; }

        // Effect add to damage receive
        public List<Effect> damageReceiveExtraEffect { get; set; }

        public List<SpellComponent> activeSpellComponents;
        public List<Entity> entityInRange;

        public EntityPrefab entityPrefab;

        public Spell basicAttack { get; set; }
        public Spell basicDefense { get; set; }
        public List<Spell> spells { get; set; }

        private BehaviorType BehaviorType { get; set; }
        private AttackBehaviorType AttackBehaviorType { get; set; }

        public bool doingSkill { get; set; } = false;

        public int nbCharges;

        public BehaviorType GetBehaviorType()
        {
            return BehaviorType;
        }
        
        public AttackBehaviorType GetAttackBehaviorType()
        {
            return AttackBehaviorType;
        }
        
        public TypeEntity GetTypeEntity()
        {
            return typeEntity;
        }
        
        public void SetBehaviorType(BehaviorType bType)
        {
            BehaviorType = bType;
        }
        
        public void SetAttackBehaviorType(AttackBehaviorType abType)
        {
            AttackBehaviorType = abType;
        }
        
        public void SetTypeEntity(TypeEntity tEntity)
        {
            typeEntity = tEntity;
        }

        public bool EntityIsUnderEffect(TypeEffect typeEffect)
        {
            return underEffects.Exists(effect => effect.typeEffect == typeEffect);
        }
        
        public Effect TryGetEffectInUnderEffect(TypeEffect typeEffect)
        {
            if (EntityIsUnderEffect(typeEffect))
            {
                return underEffects.First(effect => effect.typeEffect == typeEffect);
            }

            return null;
        }

        public void ClearUnderEffect()
        {
            underEffects.Clear();
        }

        public int GetNbUnderEffect()
        {
            return underEffects.Count;
        }

        public void AddEffectInUnderEffect(Effect effect)
        {
            underEffects.Add(effect);
        }

        public void RemoveUnderEffect(TypeEffect typeEffect)
        {
            Effect effectToDelete = TryGetEffectInUnderEffect(typeEffect);
            underEffects.Remove(effectToDelete);
        }

        public List<Effect> GetUnderEffects()
        {
            return underEffects;
        }

        public virtual void BasicAttack()
        {
            
        }

        public virtual void CancelBasicAttack()
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
            armors = new List<Armor>();
            underEffects = new List<Effect>();
            damageDealExtraEffect = new List<Effect>();
            damageReceiveExtraEffect = new List<Effect>();
            entityInRange = new List<Entity>();
            activeSpellComponents = new List<SpellComponent>();
            spells = new List<Spell>();
        }

        // Take true damage is usefull with effect pierce
        public virtual void TakeDamage(float initialDamage, Entity originDamage, DamageType damageType, SpellComponent originSpellComponent = null)
        {
            float damageReceived = (initialDamage - def) > 0 ? (initialDamage - def) : 0;
            bool takeTrueDamage = originDamage.canPierce;

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
                 playerInBack.Contains(originDamage.IdEntity)
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
                originDamage.hp += damageReceived * originDamage.TryGetEffectInUnderEffect(TypeEffect.LifeSteal).level;
                if (originDamage.hp > originDamage.initialHp)
                {
                    originDamage.hp = originDamage.initialHp;
                }
            }

            if (hasRedirection && DataObject.invocationsInScene.Count > 0)
            {
                DataObject.invocationsInScene[0].ApplyDamage(damageReceived * 0.75f, originSpellComponent);
                ApplyDamage(damageReceived * 0.25f, originSpellComponent);
            }
            else
            {
                ApplyDamage(damageReceived, originSpellComponent);
            }

            List<Effect> effects = damageReceiveExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect).ToList();
            foreach (Effect effect in effects)
            {
                EffectController.ApplyEffect(this, effect, originDamage, originDamage.entityPrefab.transform.position);
            }

            if (isSleep)
            {
                EffectController.StopCurrentEffect(this, TryGetEffectInUnderEffect(TypeEffect.Sleep));
            }

            SpellInterpreter.TriggerWhenEntityReceivedDamage(activeSpellComponents);
        }

        public virtual void ApplyDamage(float directDamage, SpellComponent originSpellComponent = null)
        {
            //Debug.Log(modelName + " - Damage applied = " + directDamage);
            hp -= directDamage;
            
            if (hp <= 0)
            {
                if (originSpellComponent != null)
                {
                    SpellInterpreter.TriggerWhenEntityDie(originSpellComponent);
                }
                
                if (shooldResurrect)
                {
                    hp = initialHp / 2;
                    EffectController.StopCurrentEffect(this, TryGetEffectInUnderEffect(TypeEffect.Resurrection));

                    return;
                }

                entityPrefab.EntityDie();

                if (isPlayer)
                {
                    TowersWebSocket.TowerSender("OTHERS", NetworkingController.CurrentRoomToken, "Player", "SendDeath", null);
                    TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken, "Player", "HasWon", null);
                    Debug.Log("Vous êtes mort");
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }
}