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
//        [SerializeField] private BuffController buffController;
//        [SerializeField] private AreaOfEffectController areaOfEffectController;
//        [SerializeField] private MovementController movementController;
//        [SerializeField] private WaveController waveController;
//        [SerializeField] private ProjectileController projectileController;
//        [SerializeField] private SummonController summonController;
//        [SerializeField] private PassiveController passiveController;
//        [SerializeField] private TransformationController transformationController;

        [SerializeField] private SpellInterpreter spellInterpreter;

        public static SpellController instance;

        private void Start()
        {
            instance = this;
            SpellInterpreter.instance = spellInterpreter;
        }

        public static bool CastSpell(Entity entity, Spell spell, int spellSlotIfPlayer = 0)
        {
            if (spell == null && spell.cost <= entity.ressource1 - spell.cost)
            {
                return false;
            }
            
//            if ((!spell.isOnCooldown && spell.nbUse != 0) || (spell.activeSpellComponent != null && spell.activeSpellComponent.isBasicAttack && entity.weapons.Count > 0))
            if ((!spell.isOnCooldown && spell.nbUse != 0) || (spell.activeSpellComponent != null && entity.weapons.Count > 0))
            {
                Debug.Log("Launch spell " + spell.nameSpell);
                if (spell.nbUse > 0)
                {
                    spell.nbUse--;
                }

                spell.isOnCooldown = true;
                instance.StartCoroutine(PlayCastTime(entity, spell, spellSlotIfPlayer));
            }
            else if (spell.canCastDuringCast)
            {
                spell.wantToCastDuringCast = true;
            }
            else if (spell.canRecast && !spell.alreadyRecast && entity.canRecast)
            {
                spell.alreadyRecast = true;
                instance.StartCoroutine(PlayCastTime(entity, spell));
            }
            else
            {
                return false;
            }

            return true;
        }

//        private static void SetOriginalPosition(SpellComponent spellComponent, Vector3 startPosition, Entity caster, Entity target = null)
//        {
//            switch (spellComponent.OriginalPosition)
//            {
//                case OriginalPosition.Caster:
//                    spellComponent.startPosition = caster.entityPrefab.transform.position;
//                    if (spellComponent.needPositionToMidToEntity)
//                    {
//                        spellComponent.startPosition += Vector3.up * (caster.entityPrefab.transform.localScale.y / 2);
//                    }
//                    break;
//                case OriginalPosition.Target:
//                    if (target != null)
//                    {
//                        spellComponent.startPosition = target.entityPrefab.transform.position;
//                        if (spellComponent.needPositionToMidToEntity)
//                        {
//                            spellComponent.startPosition += Vector3.up * (target.entityPrefab.transform.localScale.y / 2);
//                        }
//                    }
//                    else
//                    {
//                        spellComponent.startPosition = startPosition;
//                    }
//                    break;
//                case OriginalPosition.PositionInParameter:
//                    spellComponent.startPosition = startPosition;
//                    break;
//            }
//
//            switch (spellComponent.OriginalDirection)
//            {
//                case OriginalDirection.Forward:
//                    spellComponent.initialRotation = caster.entityPrefab.transform.localEulerAngles;
//                    spellComponent.trajectoryNormalized = caster.entityPrefab.transform.forward;
//                    break;
//                case OriginalDirection.Random:
//                    Vector3 rand = new Vector3();
//                    rand.x = 0;
//                    rand.y = Random.Range(0, 360);
//                    rand.z = 0;
//                    spellComponent.initialRotation = rand;
//                    spellComponent.trajectoryNormalized = rand.normalized;
//                    break;
//            }
//        }

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
                    if (lastSpellComponent == null || lastSpellComponent.spellPrefabController == null)
                    {
                        break;
                    }

                    int randEnemy = Random.Range(0, lastSpellComponent.spellPrefabController.enemiesTouchedBySpell.Count);
                    targetFound.position = lastSpellComponent.spellPrefabController.enemiesTouchedBySpell[randEnemy].entityPrefab.transform
                        .position;
                    targetFound.target = lastSpellComponent.spellPrefabController.enemiesTouchedBySpell[randEnemy];
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
                        Monster closestMonster = DataObject.monsterInScene.OrderByDescending(monster =>
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

                    targetFound.targets = lastSpellComponent.spellPrefabController.enemiesTouchedBySpell;
                    break;
            }

            return targetFound;
        }

        public static void CastSpellComponentFromTargetsFound(Entity caster, SpellComponent spellComponent, TargetsFound targetsFound, SpellComponent lastSpellComponent = null)
        {
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target => CastSpellComponent(caster, spellComponent, target, target.entityPrefab.transform.position, lastSpellComponent));
            }
            else if (targetsFound.target != null)
            {
                CastSpellComponent(caster, spellComponent, targetsFound.target, targetsFound.position, lastSpellComponent);
            }
            else if (targetsFound.position != Vector3.negativeInfinity)
            {
                CastSpellComponent(caster, spellComponent, caster.entityPrefab.target, targetsFound.position, lastSpellComponent);
            }
        }

        public static SpellComponent CastSpellComponent(Entity caster, SpellComponent spellComponent, Entity target, Vector3 startPosition, SpellComponent lastSpellComponent = null)
        {
            SpellComponent cloneSpellComponent = Tools.Clone(spellComponent);
            caster.activeSpellComponents.Add(cloneSpellComponent);
            
            cloneSpellComponent.caster = caster;
            cloneSpellComponent.targetAtCast = target;

            if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_TARGET)
            {
                cloneSpellComponent.trajectory.objectToFollow = target.entityPrefab.transform;
            } 
            else if (cloneSpellComponent.trajectory.followCategory == FollowCategory.FOLLOW_LAST_SPELL && lastSpellComponent != null && lastSpellComponent.spellPrefabController != null)
            {
                cloneSpellComponent.trajectory.objectToFollow = lastSpellComponent.spellPrefabController.transform;
            }

            SpellInterpreter.instance.StartSpellTreatment(cloneSpellComponent, startPosition);
            return cloneSpellComponent;
        }

        public static IEnumerator StartCooldown(Entity caster, Spell spell, int spellSlotIfPlayer = 0)
        {
            GameObject bgTimer = null;
            Text timer = null;
            
            if (spellSlotIfPlayer > 0)
            {
                PlayerPrefab playerPrefab = caster.entityPrefab as PlayerPrefab;

                switch (spellSlotIfPlayer)
                {
                    case 1:
                        bgTimer = playerPrefab.bgTimer1;
                        timer = playerPrefab.timer1;
                        break;
                    case 2:
                        bgTimer = playerPrefab.bgTimer2;
                        timer = playerPrefab.timer2;
                        break;
                    case 3:
                        bgTimer = playerPrefab.bgTimer3;
                        timer = playerPrefab.timer3;
                        break;
                }

                if (timer != null && bgTimer != null)
                {
                    bgTimer.SetActive(true);
                    timer.text = spell.cooldown.ToString();
                }
            }

            float duration = spell.cooldown;
            while (duration > 0)
            {
                yield return new WaitForSeconds(1);
                duration -= 1;

                if (timer != null)
                {
                    timer.text = duration.ToString();
                }
            }

            if (bgTimer != null)
            {
                bgTimer.SetActive(false);
            }

            spell.isOnCooldown = false;
            spell.alreadyRecast = false;
        }

        public static IEnumerator PlayCastTime(Entity caster, Spell spell, int spellSlotIfPlayer = 0)
        {
            TargetsFound targetsFound = GetTargetGetWithStartForm(caster, spell.startFrom);
            caster.ressource1 -= spell.cost;
            
            float duration = spell.castTime;

            if (spell.duringCastSpellComponent != null)
            {
                spell.canCastDuringCast = true;
            }

            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;

                if (spell.wantToCastDuringCast)
                {
                    spell.wantToCastDuringCast = false;
                    spell.canCastDuringCast = false;

                    CastSpellComponentFromTargetsFound(caster, spell.duringCastSpellComponent, targetsFound);
                    if (spell.interruptCurrentCast)
                    {
                        yield break;
                    }
                }
            }

            if (spell.activeSpellComponent == null)
            {
                yield break;
            }

            instance.StartCoroutine(StartCooldown(caster, spell, spellSlotIfPlayer));
            CastSpellComponentFromTargetsFound(caster, spell.activeSpellComponent, targetsFound);
        }

        public static void CastPassiveSpell(Entity entity)
        {
            if (entity.spells == null)
            {
                return;
            }

            foreach (Spell spell in entity.spells)
            {
                if (spell.passiveSpellComponent != null)
                {
                    // TODO : Reimplement Passive spell
//                    CastSpellComponent(entity, spell.passiveSpellComponent, entity.entityPrefab.target);
                }
            }
        }

        public static Spell LoadSpellByName(string nameSpell)
        {
            string path = "Assets/Data/SpellsJson/" + nameSpell + ".json";
            Spell spell = FindSpellWithPath(path);

            if (spell == null)
            {
                Debug.Log("Pas de spells");
            }

            return spell;
        }
        
        private static Spell FindSpellWithPath(string tempPath)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;
            Spell spell = null;
            string jsonSpell;

            try
            {
                jsonSpell = File.ReadAllText(tempPath);
                data = fsJsonParser.Parse(jsonSpell);
                serializer.TryDeserialize(data, ref spell);
                spell = Tools.Clone(spell);
            }
            catch (Exception e)
            {
//                Debug.Log("Cant import spell for path : " + tempPath);
//                Debug.Log(e.Message);
            }

            return spell;
        }
    }
}