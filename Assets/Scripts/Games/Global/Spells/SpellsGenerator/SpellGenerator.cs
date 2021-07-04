using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.Global.Spells.SpellsGenerator
{
    public class SpellGenerator
    {
        private static bool isAnArea = false;
        private static bool isHeal = false;
        private static bool isSupport  = false;
        private static bool isDamage = false;
        private static bool isCac = false;
        private static bool isDistance = false;

        private static void ResetParams(bool isHeal,
            bool isSupport,
            bool isDamage,
            bool isCac,
            bool isDistance)
        {
            SpellGenerator.isHeal = isHeal;
            SpellGenerator.isSupport = isSupport;
            SpellGenerator.isDamage = isDamage;
            SpellGenerator.isDistance = isDistance;
            SpellGenerator.isCac = isCac;

            if (Random.Range(0, 1) == 0)
            {
                isAnArea = true;
            }
        }
        
        public static Spell GenerateSpellWithParameter(
            PathCreator path,
            bool isHeal,
            bool isSupport,
            bool isDamage,
            bool isCac,
            bool isDistance)
        {
            ResetParams(isHeal, isSupport, isDamage, isCac, isDistance);
            
            SpellComponent activeSpellComponent = GenerateActiveSpellComponent(path);

            StartFrom startFrom = StartFrom.Caster;
            if (isDistance && isAnArea)
            {
                startFrom = StartFrom.CursorTarget;
            }

            Spell spell = new Spell
            {
                spellComponentFirstActivation = activeSpellComponent,
                cooldown = Random.Range(1, 4),
                cost = Random.Range(1, 10),
                castTime = 0,
                nameSpell = "SpellGenerateAt_" + DateTime.Now,
                startFrom = startFrom,
            };

            return spell;
        }

        private static SpellComponent GenerateActiveSpellComponent(PathCreator path)
        {
            Trajectory trajectory = GenerateTrajectory(path);

            SpellToInstantiate spellToInstantiate = GenerateSpellToInstantiate();

            Dictionary<Trigger, List<ActionTriggered>> actions = GenerateSpellAction();

            SpellComponent spellComponent = new SpellComponent
            {
                actions = actions,
                trajectory = trajectory,
                spellToInstantiate = spellToInstantiate,
                spellDuration = isDistance ? 1 : Random.Range(4, 10),
                spellInterval = Random.Range(0.5f, 2f)
            };

            return spellComponent;
        }

        private static Trajectory GenerateTrajectory(PathCreator path)
        {
            Trajectory trajectory = null;

            if (isDistance)
            {
                path.bezierPath.SetPoint(1, Vector3.forward * Random.Range(10, 20));

                trajectory = new Trajectory
                {
                    followCategory = FollowCategory.NONE,
                    speed = Random.Range(5, 20),
                    spellPath = path.bezierPath,
                    endOfPathInstruction = EndOfPathInstruction.Stop,
                    disapearAtTheEndOfTrajectory = true
                };
            }
            else if (isAnArea)
            {
                trajectory = new Trajectory
                {
                    followCategory = FollowCategory.FOLLOW_TARGET,
                };
            }

            return trajectory;
        }

        private static SpellToInstantiate GenerateSpellToInstantiate()
        {
            int scaleX = isDistance && !isAnArea ? Random.Range(1, 5) : Random.Range(2, 7);
            int scaleZ = isDistance && !isAnArea ? Random.Range(1, 5) : Random.Range(4, 10);

            return new SpellToInstantiate
            {
                geometry = (Geometry) Random.Range(0, 2),
                offsetStartPosition = Vector3.zero,
                scale = new Vector3(scaleX, 1, scaleZ),
                passingThroughEntity = true,
                pathGameObjectToInstantiate = GetModelFromParameters()
            };
        }

        private static Dictionary<Trigger, List<ActionTriggered>> GenerateSpellAction()
        {
            Dictionary<Trigger, List<ActionTriggered>> actions = new Dictionary<Trigger, List<ActionTriggered>>();

            if (isDamage)
            {
                Trigger trigger = Trigger.ON_TRIGGER_ENTER;
                int damage = Random.Range(15, 55);

                ActionTriggered actionTriggered = new ActionTriggered
                {
                    damageDeal = damage,
                    startFrom = StartFrom.AllEnemiesInArea,
                };

                actions.Add(trigger, new List<ActionTriggered>{ actionTriggered });
            }
            else if (isHeal)
            {
                Trigger trigger = Trigger.START;

                if (isAnArea)
                {
                    trigger = Trigger.INTERVAL;
                } else if (isDistance)
                {
                    trigger = Trigger.ON_TRIGGER_ENTER;
                }

                ActionTriggered actionHeal = GenerateHealAction();
                actions.Add(trigger, new List<ActionTriggered>{ actionHeal });
            }

            return actions;
        }

        private static ActionTriggered GenerateHealAction()
        {
            StartFrom effectAppliedFor = StartFrom.Caster;
            
            if (isAnArea)
            {
                effectAppliedFor = StartFrom.AllAlliesInArea;
            } else if (isDistance)
            {
                effectAppliedFor = StartFrom.TargetEntity;
            }

            int effectChosen = Random.Range(0, 1);

            Effect effect = new Effect {level = Random.Range(1, 5)};

            if (effectChosen == 0)
            {
                effect.typeEffect = TypeEffect.Heal;
            }
            else
            {
                effect.typeEffect = TypeEffect.Regen;
                effect.durationInSeconds = Random.Range(3, 5);
            }

            return new ActionTriggered
            {
                damageDeal = 0,
                effect = effect,
                startFrom = effectAppliedFor
            };
        }

        private static string GetModelFromParameters()
        {
            string model;
            
            if (isAnArea)
            {
                model = "Spells/FX_Fire_Big_03";
            } else if (isDistance)
            {
                model = "Spells/SkeletonMageFireball";
            }
            else
            {
                model = "FX_Swirl_Fast_01";
            }

            return model;
        }
    }
}