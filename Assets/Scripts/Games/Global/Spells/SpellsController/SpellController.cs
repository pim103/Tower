using System;
using System.Collections;
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
    public enum StartFrom
    {
        Caster,
        TargetEntity,
        CursorTarget,
        RandomPositionInArea,
        RandomEnemyInArea,
        LastSpellComponent,
        ClosestEnemyFromCaster
    }

    public enum OriginalDirection
    {
        None,
        Forward,
        Random
    }

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

        public static Vector3 FindStartPosition(SpellComponent newSpellComponent, SpellComponent lastSpellComponent = null)
        {
            switch (newSpellComponent.startFrom)
            {
                case StartFrom.Caster:
                    return newSpellComponent.caster.entityPrefab.transform.position;
                case StartFrom.TargetEntity:
                    return newSpellComponent.targetAtCast.entityPrefab.transform.position;
                case StartFrom.RandomPositionInArea:
                    if (lastSpellComponent == null)
                    {
                        return Vector3.one;
                    }
                    
                    SpellPrefabController spellPrefabController = lastSpellComponent.spellPrefabController;
                    SpellToInstantiate spellToInstantiate = lastSpellComponent.spellToInstantiate;
                    Vector3 currentPosition = spellPrefabController.transform.position;
                    
                    if (lastSpellComponent.spellToInstantiate.geometry == Geometry.Sphere)
                    {
                        float t = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);
                        float rx = Random.Range(0.0f, spellToInstantiate.scale.x / 2);
                        float rz = Random.Range(0.0f, spellToInstantiate.scale.z / 2);
                        return new Vector3
                        {
                            x = currentPosition.x + rx * Mathf.Cos(t), 
                            y = currentPosition.y, 
                            z = currentPosition.z + rz * Mathf.Sin(t)
                        };
                    }
                    else
                    {
                        return new Vector3
                        {
                            x = currentPosition.x + Random.Range(-spellToInstantiate.scale.x/2, spellToInstantiate.scale.x/2), 
                            y = currentPosition.y, 
                            z = currentPosition.z + Random.Range(-spellToInstantiate.scale.z/2, spellToInstantiate.scale.z/2)
                        };
                    }
                case StartFrom.RandomEnemyInArea:
                    if (lastSpellComponent == null)
                    {
                        return Vector3.one;
                    }

                    int randEnemy = Random.Range(0, lastSpellComponent.spellPrefabController.enemiesTouchedBySpell.Count);
                    return lastSpellComponent.spellPrefabController.enemiesTouchedBySpell[randEnemy].entityPrefab.transform
                        .position;
                case StartFrom.LastSpellComponent:
                    if (lastSpellComponent == null)
                    {
                        return Vector3.one;
                    }

                    return lastSpellComponent.spellPrefabController.transform.position;
                case StartFrom.ClosestEnemyFromCaster:
                    if (newSpellComponent.caster.isPlayer)
                    {
                        Monster closestMonster = DataObject.monsterInScene.OrderByDescending(monster =>
                            newSpellComponent.caster.entityPrefab.transform.position -
                            monster.entityPrefab.transform.position).First();
                    }
                    break;
                case StartFrom.CursorTarget:
                    return newSpellComponent.caster.entityPrefab.positionPointed;
            }
            
            return Vector3.one;
        }

        public static void CastSpellComponent(Entity entity, SpellComponent spellComponent, Entity target, SpellComponent originSpellComponent = null)
        {
            spellComponent.caster = entity;
            spellComponent.targetAtCast = target;

            Vector3 initialPosition = FindStartPosition(spellComponent, originSpellComponent);

            instance.spellInterpreter.StartSpellTreatment(spellComponent, initialPosition);
        }

        public static IEnumerator StartCooldown(Entity entity, Spell spell, int spellSlotIfPlayer = 0)
        {
            GameObject bgTimer = null;
            Text timer = null;
            
            if (spellSlotIfPlayer > 0)
            {
                PlayerPrefab playerPrefab = entity.entityPrefab as PlayerPrefab;

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

        public static IEnumerator PlayCastTime(Entity entity, Spell spell, int spellSlotIfPlayer = 0)
        {
            entity.ressource1 -= spell.cost;
            
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
                    
                    CastSpellComponent(entity, spell.duringCastSpellComponent, entity.entityPrefab.target);
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
            
            instance.StartCoroutine(StartCooldown(entity, spell, spellSlotIfPlayer));
            CastSpellComponent(entity, spell.activeSpellComponent, entity.entityPrefab.target);
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
                    CastSpellComponent(entity, spell.passiveSpellComponent, entity.entityPrefab.target);
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