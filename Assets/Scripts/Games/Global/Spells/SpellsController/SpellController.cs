using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using FullSerializer;
using Games.Global.Entities;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
using Games.Players;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Games.Global.Spells.SpellsController
{

    public class SpellController : MonoBehaviour
    {
        [SerializeField] private SpellInterpreter spellInterpreter;

        public static SpellController instance;

        private void Start()
        {
            instance = this;
            SpellInterpreter.instance = spellInterpreter;
        }

        public static bool CastSpell(Entity entity, Spell spell)
        {
            if (spell == null || 
                entity.ressource - spell.cost < 0 ||
                spell.isOnCooldown ||
                spell.nbUse == 0
            ) {
                return false;
            }

            if (spell.nbUse > 0)
            {
                spell.nbUse--;
            }

            Debug.Log("Launch spell " + spell.nameSpell);
            switch (spell.TypeSpell)
            {
                case TypeSpell.Holding:
                    spell.isHolding = true;
                    break;
                case TypeSpell.HoldThenRelease:
                    spell.isHolding = true;
                    break;
                case TypeSpell.ActivePassive:
                    instance.StartCoroutine(StartCooldown(spell));
                    break;
                case TypeSpell.UniqueActivation:
                    instance.StartCoroutine(StartCooldown(spell));
                    break;
                case TypeSpell.Passive:
                    break;
            }
            
            if (entity.GetTypeEntity() == TypeEntity.ALLIES)
            {
                if (entity.weapon?.category?.id == 4)
                {
                    entity.entityPrefab.animator.SetTrigger("doingBowAttack");
                    entity.entityPrefab.audioSource.PlayOneShot(entity.entityPrefab.bowAttackClip);
                }
                else if (entity.weapon?.category?.id == 5)
                {
                    entity.entityPrefab.animator.SetTrigger("doingShortSwordAttack");
                    entity.entityPrefab.audioSource.PlayOneShot(entity.entityPrefab.swordAttackClip);
                }

                //entity.entityPrefab.animator.ResetTrigger("doingShortSwordAttack");
            }
            else
            {
                if (entity.weapon?.category?.id == 4)
                {
                    //entity.entityPrefab.audioSource.PlayOneShot(entity.entityPrefab.bowAttackClip);
                    entity.entityPrefab.animator.SetTrigger("DistanceAttack");
                }
                else
                {
                    entity.entityPrefab.animator.SetTrigger("MeleeAttack");
                }
            }
            instance.StartCoroutine(PlayCastTime(entity, spell));

            return true;
        }

        public static void InterruptSpell(Spell spell)
        {
            if (spell.isOnCooldown)
            {
                return;
            }
            
            if (spell.TypeSpell == TypeSpell.Holding || spell.TypeSpell == TypeSpell.HoldThenRelease)
            {
                instance.StartCoroutine(StartCooldown(spell));
            }

            spell.isHolding = false;
        }

        private static IEnumerator PlayCastTime(Entity caster, Spell spell)
        {
            TargetsFound targetsFound = GetTargetGetWithStartForm(caster, spell.startFrom);
            caster.ressource -= spell.cost;
            
            float duration = spell.castTime;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }

            if (spell.TypeSpell == TypeSpell.MultipleActivation)
            {
                yield return PlayMultipleAction(caster, spell, targetsFound);
                yield break;
            }

            CastSpellComponentFromTargetsFound(caster, spell, spell.spellComponentFirstActivation, targetsFound);

            if (spell.TypeSpell == TypeSpell.Holding || spell.TypeSpell == TypeSpell.HoldThenRelease)
            {
                spell.timeHolding = 0.0f;

                while ( 
                    spell.isHolding &&
                    caster.ressource - spell.holdingCost >= 0 &&
                    (spell.timeHolding < spell.maximumTimeHolding || spell.maximumTimeHolding == 0))
                {
                    yield return new WaitForSeconds(0.1f);
                    spell.timeHolding += 0.1f;
                    caster.ressource -= spell.holdingCost;
                }

                InterruptSpell(spell);
                CastSpellComponentFromTargetsFound(caster, spell, spell.spellComponentAfterHolding, targetsFound);
            }
        }

        private static IEnumerator PlayMultipleAction(Entity caster, Spell spell, TargetsFound targetsFound)
        {
            SpellComponent spellComponentToLaunch = null;
            int currentSpellIndexLaunch = spell.nbSpellActivation;

            if (spell.nbSpellActivation == 0)
            {
                spellComponentToLaunch = spell.spellComponentFirstActivation;
            }
            else if (spell.nbSpellActivation < spell.spellComponents.Count)
            {
                spellComponentToLaunch = spell.spellComponents[spell.nbSpellActivation].spellComponent;
            }

            CastSpellComponentFromTargetsFound(caster, spell, spellComponentToLaunch, targetsFound);
            spell.nbSpellActivation += 1;

            // If index == size of spellComponents, last component was played, start cooldown
            if (spell.nbSpellActivation >= spell.spellComponents.Count)
            {
                yield return StartCooldown(spell);
                yield break;
            }

            float timeBeforeRecast = spell.spellComponents[spell.nbSpellActivation].timeBeforeReset;

            while (timeBeforeRecast > 0 && currentSpellIndexLaunch == spell.nbSpellActivation)
            {
                yield return new WaitForSeconds(0.1f);
                timeBeforeRecast -= 0.1f;
            }

            // If same index, player doesn't recast spell, so start cooldown
            if (currentSpellIndexLaunch == spell.nbSpellActivation)
            {
                yield return StartCooldown(spell);
            }
        }

        private static IEnumerator StartCooldown(Spell spell)
        {
            spell.isOnCooldown = true;
            spell.currentCooldown = spell.cooldown;

            while (spell.currentCooldown > 0)
            {
                yield return new WaitForSeconds(0.1f);
                spell.currentCooldown -= 0.1f;
            }

            spell.isOnCooldown = false;
        }

        public static void CastSpellComponentFromTargetsFound(Entity caster, Spell spell, SpellComponent spellComponent, TargetsFound targetsFound, SpellComponent lastSpellComponent = null)
        {
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target => CastSpellComponent(caster, spellComponent, target, target.entityPrefab.transform.position, spell, lastSpellComponent));
            }
            else if (targetsFound.target != null)
            {
                CastSpellComponent(caster, spellComponent, targetsFound.target, targetsFound.position, spell, lastSpellComponent);
            }
            else if (targetsFound.position != Vector3.negativeInfinity)
            {
                CastSpellComponent(caster, spellComponent, caster.entityPrefab.target, targetsFound.position, spell, lastSpellComponent);
            }
        }

        public static SpellComponent CastSpellComponent(
            Entity caster,
            SpellComponent spellComponent,
            Entity target,
            Vector3 startPosition,
            Spell originSpell,
            SpellComponent lastSpellComponent = null)
        {
            if (spellComponent == null)
            {
                return null;
            }
            
            SpellComponent cloneSpellComponent = CloneCorrectSpellComponent(spellComponent);
            cloneSpellComponent.originSpell = originSpell;
            caster.activeSpellComponents.Add(cloneSpellComponent);

            cloneSpellComponent.caster = caster;
            cloneSpellComponent.targetAtCast = target;

            if (cloneSpellComponent.trajectory != null)
            {
                if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_TARGET)
                {
                    cloneSpellComponent.trajectory.objectToFollow = target.entityPrefab.transform;
                } 
                else if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_LAST_SPELL && lastSpellComponent != null && lastSpellComponent.spellPrefabController != null)
                {
                    cloneSpellComponent.trajectory.objectToFollow = lastSpellComponent.spellPrefabController.transform;
                }
            }

            SpellInterpreter.instance.StartSpellTreatment(cloneSpellComponent, startPosition);
            return cloneSpellComponent;
        }

        public static void CastPassiveSpell(Entity entity)
        {
            if (entity.spells == null || entity.spells.Count == 0)
            {
                return;
            }

            List<Spell> passiveSpell = entity.spells.FindAll(spell => spell.TypeSpell == TypeSpell.Passive);

            foreach (Spell spell in passiveSpell)
            {
                if (spell.spellComponentFirstActivation != null)
                {
                    // TODO : Reimplement Passive spell
//                    CastSpellComponent(entity, spell.passiveSpellComponent, entity.entityPrefab.target);
                }
            }
        }

        private static SpellComponent CloneCorrectSpellComponent(SpellComponent spellComponent)
        {
            switch (spellComponent.TypeSpellComponent)
            {
                case TypeSpellComponent.Movement:
                    return Tools.Clone(spellComponent as MovementSpell);
                case TypeSpellComponent.Transformation:
                    return Tools.Clone(spellComponent as TransformationSpell);
                case TypeSpellComponent.Passive:
                    return Tools.Clone(spellComponent as PassiveSpell);
                case TypeSpellComponent.BasicAttack:
                    return Tools.Clone(spellComponent as BasicAttackSpell);
                case TypeSpellComponent.Summon:
                    return Tools.Clone(spellComponent as SummonSpell);
                default:
                    return Tools.Clone(spellComponent);
            }
        }

        public static TargetsFound GetTargetGetWithStartForm(Entity caster, StartFrom startFromNewSpellComponent,
            SpellComponent lastSpellComponent = null)
        {
            TargetsFound targetFound = new TargetsFound();

            switch (startFromNewSpellComponent)
            {
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.Caster:
                    targetFound.position = caster.entityPrefab.transform.position;
                    targetFound.target = caster;
                    break;
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.TargetEntity:
                    if (lastSpellComponent == null)
                    {
                        targetFound.position = caster.entityPrefab.target.entityPrefab.transform.position;
                        targetFound.target = caster.entityPrefab.target;
                    }
                    else
                    {
                        targetFound.position = lastSpellComponent.targetAtCast.entityPrefab.transform.position;
                        targetFound.target = lastSpellComponent.targetAtCast;
                    }

                    if (targetFound.target != null && targetFound.target.hp <= 0)
                    {
                        targetFound.target = null;
                        targetFound.position = Vector3.zero;
                    }
                    
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.RandomPositionInArea:
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null)
                    {
                        break;
                    }

                    SpellPrefabController spellPrefabController = lastSpellComponent.spellPrefabController;
                    SpellToInstantiate spellToInstantiate = lastSpellComponent.spellToInstantiate;
                    Vector3 currentPosition = spellPrefabController.transform.position;

                    if (lastSpellComponent.spellToInstantiate.geometry == Geometry.Sphere)
                    {
                        float t = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);
                        float rx = Random.Range(0.0f, spellToInstantiate.scale.x / 2);
                        float rz = Random.Range(0.0f, spellToInstantiate.scale.z / 2);
                        targetFound.position = new Vector3
                        {
                            x = currentPosition.x + rx * Mathf.Cos(t), 
                            y = currentPosition.y, 
                            z = currentPosition.z + rz * Mathf.Sin(t)
                        };
                    }
                    else
                    {
                        targetFound.position = new Vector3
                        {
                            x = currentPosition.x + Random.Range(-spellToInstantiate.scale.x/2, spellToInstantiate.scale.x/2), 
                            y = currentPosition.y, 
                            z = currentPosition.z + Random.Range(-spellToInstantiate.scale.z/2, spellToInstantiate.scale.z/2)
                        };
                    }

                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.RandomEnemyInArea:
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null || lastSpellComponent.spellPrefabController.enemiesTouchedBySpell.Count == 0)
                    {
                        break;
                    }

                    List<Entity> enemiesAlive =
                        lastSpellComponent.spellPrefabController.enemiesTouchedBySpell.FindAll(enemy => enemy.hp > 0);

                    if (enemiesAlive.Count == 0)
                    {
                        break;
                    }

                    int randEnemy = Random.Range(0, enemiesAlive.Count);
                    targetFound.position = enemiesAlive[randEnemy].entityPrefab.transform.position;
                    targetFound.target = enemiesAlive[randEnemy];
                    break;
                /* Can be use by ActionTriggered */
                case StartFrom.LastSpellComponent:
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null)
                    {
                        break;
                    }

                    targetFound.position = lastSpellComponent.spellPrefabController.transform.position;
                    break;
                /* Can be use by Spell and ActionTriggered */
                case StartFrom.ClosestEnemyFromCaster:
                    if (caster.isPlayer)
                    {
                        Monster closestMonster = DataObject.monsterInScene.FindAll(monster => monster.hp > 0).OrderByDescending(monster =>
                            caster.entityPrefab.transform.position -
                            monster.entityPrefab.transform.position).First();

                        targetFound.position = closestMonster.entityPrefab.transform.position;
                        targetFound.target = closestMonster;
                    }
                    break;
                /* Can be use by Spell */
                case StartFrom.CursorTarget:
                    targetFound.position = caster.entityPrefab.positionPointed;
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.AllAlliesInArea:
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null)
                    {
                        break;
                    }
                    
                    targetFound.targets = lastSpellComponent.spellPrefabController.alliesTouchedBySpell;
                    break;
                /* Can be use by ActionTriggered after area */
                case StartFrom.AllEnemiesInArea:
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null)
                    {
                        break;
                    }

                    targetFound.targets = lastSpellComponent.spellPrefabController.enemiesTouchedBySpell.FindAll(enemy => enemy.hp > 0);
                    break;
            }

            return targetFound;
        }
    }
}