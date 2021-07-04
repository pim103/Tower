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
        private static bool isOffensiveBuff = false;
        private static bool isDefensiveBuff = false;

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
            SpellGenerator.isOffensiveBuff = false;
            SpellGenerator.isDefensiveBuff = false;

            if (Random.Range(0, 2) == 0)
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
                cooldown = Random.Range(1, 10),
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
            // Une chance sur 2 que ça suive le caster
            else if (isAnArea && isCac && Random.Range(0,1) == 0)
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

                ActionTriggered actionTriggered = GenerateDamageAction();

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
            else if (isSupport)
            {
                Trigger trigger = Trigger.START;
                ActionTriggered actionSupport = GenerateSupportAction();
                actions.Add(trigger, new List<ActionTriggered>{ actionSupport });
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

            Effect effect = EffectForAction();
            
            return new ActionTriggered
            {
                damageDeal = 0,
                actionOnEffectType = ActionOnEffectType.ADD,
                effect = effect,
                startFrom = effectAppliedFor
            };
        }

        private static ActionTriggered GenerateDamageAction()
        {
            Effect damageEffect = null;
            // Ajouter un effect
            if (Random.Range(0, 3) == 0)
            {
                damageEffect = EffectForAction();
            }

            int damage = damageEffect != null ? Random.Range(5, 15) : Random.Range(15, 30);

            return new ActionTriggered
            {
                damageDeal = damage,
                actionOnEffectType = ActionOnEffectType.ADD,
                effect = damageEffect,
                startFrom = StartFrom.AllEnemiesInArea,
            };
        }

        private static ActionTriggered GenerateSupportAction()
        {
            // Définie si c'est un buff offensif ou non
            isOffensiveBuff = Random.Range(0, 2) == 0;
            isDefensiveBuff = !isOffensiveBuff;

            // Si un effet s'ajoute aux attaques de bases, ou aux dégats reçus
            bool isExtraEffectBuff = Random.Range(0, 2) == 0;

            Effect effect = EffectForAction(isExtraEffectBuff);

            ActionOnEffectType effectTypeResolution = isExtraEffectBuff
                ? isOffensiveBuff ? ActionOnEffectType.BUFF_ATTACK : ActionOnEffectType.BUFF_DEFENSE
                : ActionOnEffectType.ADD;

            return new ActionTriggered
            {
                damageDeal = 0,
                actionOnEffectType = effectTypeResolution,
                effect = effect,
                startFrom = StartFrom.AllEnemiesInArea,
            };
        }

        private static Effect EffectForAction(bool isForSupportExtraEffectSpell = false)
        {
            Effect effect = new Effect {level = Random.Range(1, 5)};

            if (isHeal)
            {
                // 0 == Heal - 1 == Regen
                int effectChosen = Random.Range(0, 2);
                if (effectChosen == 0)
                {
                    effect.typeEffect = TypeEffect.Heal;
                }
                else
                {
                    effect.typeEffect = TypeEffect.Regen;
                }
            } else if (isDamage)
            {
                int effectChosen = Random.Range(0, 3);
                switch (effectChosen)
                {
                    case 0:
                        effect.typeEffect = TypeEffect.Burn;
                        break;
                    case 1:
                        effect.typeEffect = TypeEffect.Poison;
                        break;
                    case 2:
                        effect.typeEffect = TypeEffect.Freezing;
                        break;
                }
            } else if (isSupport)
            {
                if (isOffensiveBuff)
                {
                    int effectChosen = Random.Range(0, 2);
                    switch (effectChosen)
                    {
                        case 0:
                            effect.typeEffect = TypeEffect.AttackUp;
                            break;
                        case 1:
                            effect.typeEffect = TypeEffect.AttackSpeedUp;
                            break;
                    }
                }
                else
                {
                    int effectChosen = Random.Range(0, 2);
                    switch (effectChosen)
                    {
                        case 0:
                            effect.typeEffect = TypeEffect.SpeedUp;
                            break;
                        case 1:
                            effect.typeEffect = TypeEffect.DefenseUp;
                            break;
                    }
                }
            }

            if (isForSupportExtraEffectSpell)
            {
                effect.durationBuff = Random.Range(3, 5);
            }

            effect.durationInSeconds = Random.Range(3, 5);

            return effect;
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
                model = "Spells/FX_Swirl_Fast_01";
            }

            return model;
        }
    }
}