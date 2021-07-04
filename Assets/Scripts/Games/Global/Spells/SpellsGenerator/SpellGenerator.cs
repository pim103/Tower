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
        private static int complexity = 1;
        
        private static bool isAnArea = false;
        private static bool isHeal = false;
        private static bool isSupport  = false;
        private static bool isDamage = false;
        private static bool isCac = false;
        private static bool isDistance = false;
        private static bool isOffensiveBuff = false;
        private static bool isDefensiveBuff = false;
        private static bool isExtraBuff = false;
        private static Effect effectSaved;
        private static StartFrom targetSaved = StartFrom.Caster;

        private static StartFrom target = StartFrom.Caster;

        private static void ResetParams(bool isHeal,
            bool isSupport,
            bool isDamage,
            bool isCac,
            bool isDistance)
        {
            complexity = 1;
            SpellGenerator.isAnArea = false;
            SpellGenerator.isHeal = isHeal;
            SpellGenerator.isSupport = isSupport;
            SpellGenerator.isDamage = isDamage;
            SpellGenerator.isCac = isCac;
            SpellGenerator.isDistance = isDistance;
            SpellGenerator.isOffensiveBuff = false;
            SpellGenerator.isDefensiveBuff = false;
            SpellGenerator.isExtraBuff = false;
            effectSaved = null;
            SpellGenerator.target = StartFrom.Caster;

            if (Random.Range(0, 3) == 0)
            {
                isAnArea = true;
            }

            if (isDistance && !isAnArea)
            {
                if (Random.Range(0, 2) == 0)
                {
                    target = StartFrom.TargetEntity;
                }
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

            DisplaySpellData(spell);
            return spell;
        }

        private static SpellComponent GenerateActiveSpellComponent(PathCreator path)
        {
            Trajectory trajectory = GenerateTrajectory(path);

            Dictionary<Trigger, List<ActionTriggered>> actions = GenerateSpellAction();

            SpellToInstantiate spellToInstantiate = target != StartFrom.TargetEntity || !isDamage ? GenerateSpellToInstantiate() : null;

            SpellComponent spellComponent = new SpellComponent
            {
                actions = actions,
                trajectory = trajectory,
                spellToInstantiate = spellToInstantiate,
                spellDuration = isDistance ? 1 : isAnArea ? Random.Range(8, 15) : isCac ? Random.Range(1, 4) : Random.Range(4, 10),
                spellInterval = Random.Range(0.5f, 1f)
            };

            return spellComponent;
        }

        private static void DisplaySpellData(Spell spell)
        {
            string range = "Compétence" + (isCac ? "Cac" : "Distance");
            string category = isDamage ? "De dégat" : isHeal ? "De soin" : "De soutien"; 
            string area = isAnArea ? "en zone d'effet" : "";
            string buff = isOffensiveBuff || isDefensiveBuff ? "Avec un Buff " + (isOffensiveBuff ? "offensif" : "défensif") : "";
            string buffExtra = isExtraBuff ? isOffensiveBuff ? "les attaques de bases" : "lors de dégats reçus" : "";
            string effect = effectSaved != null ? "Avec en effet " + effectSaved.typeEffect : "";

            Debug.Log($"{range} {category} {area} {buff} {buffExtra} {effect}");
        }

        private static Trajectory GenerateTrajectory(PathCreator path)
        {
            Trajectory trajectory = null;

            // generate trajectory for distance spell and when target is not an entity
            if (isDistance && target != StartFrom.TargetEntity && !isAnArea)
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
            else if (isCac)
            {
                // Une chance sur 2 que ça suive le caster
                if (isAnArea && Random.Range(0, 2) == 0)
                {
                    trajectory = new Trajectory
                    {
                        followCategory = FollowCategory.FOLLOW_TARGET,
                    };
                }
                else
                {
                    trajectory = new Trajectory
                    {
                        followCategory = FollowCategory.FOLLOW_TARGET,
                    };
                }
            }

            return trajectory;
        }

        private static SpellToInstantiate GenerateSpellToInstantiate()
        {
            int scaleX = isDistance && !isAnArea ? Random.Range(1, 5) : Random.Range(4, 7);
            int scaleZ = isDistance && !isAnArea ? Random.Range(1, 5) : Random.Range(4, 10);

            Vector3 offset = Vector3.up;

            if (isCac && isDamage && !isAnArea)
            {
                offset += Vector3.forward;
            }
            
            return new SpellToInstantiate
            {
                geometry = (Geometry) Random.Range(0, 2),
                offsetStartPosition = offset,
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

                if (target == StartFrom.TargetEntity)
                {
                    trigger = Trigger.START;
                }

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
                if (isAnArea)
                {
                    trigger = Trigger.INTERVAL;
                } else if (isDistance)
                {
                    trigger = Trigger.ON_TRIGGER_ENTER;
                }

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
                effectAppliedFor = target;
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

            StartFrom startFrom = target == StartFrom.TargetEntity ? target : StartFrom.AllEnemiesInArea;

            return new ActionTriggered
            {
                damageDeal = damage,
                actionOnEffectType = ActionOnEffectType.ADD,
                effect = damageEffect,
                startFrom = startFrom
            };
        }

        private static ActionTriggered GenerateSupportAction()
        {
            // Définie si c'est un buff offensif ou non
            isOffensiveBuff = Random.Range(0, 2) == 0;
            isDefensiveBuff = !isOffensiveBuff;

            // Si un effet s'ajoute aux attaques de bases, ou aux dégats reçus
            isExtraBuff = Random.Range(0, 2) == 0;

            Effect effect = EffectForAction(isExtraBuff);

            ActionOnEffectType effectTypeResolution = isExtraBuff
                ? isOffensiveBuff ? ActionOnEffectType.BUFF_ATTACK : ActionOnEffectType.BUFF_DEFENSE
                : ActionOnEffectType.ADD;

            // By default, in cac
            StartFrom startFrom = StartFrom.Caster;
            if (isAnArea)
            {
                startFrom = StartFrom.AllAlliesInArea;
            } else if (isDistance)
            {
                startFrom = target;
            }

            return new ActionTriggered
            {
                damageDeal = 0,
                actionOnEffectType = effectTypeResolution,
                effect = effect,
                startFrom = startFrom,
            };
        }

        private static Effect EffectForAction(bool isForSupportExtraEffectSpell = false)
        {
            Effect effect = new Effect {level = Random.Range(1, 5)};

            if (isHeal || (isForSupportExtraEffectSpell && isDefensiveBuff))
            {
                // 0 == Heal - 1 == Regen
                int effectChosen = Random.Range(0, 2);
                if (effectChosen == 0 || !isDefensiveBuff)
                {
                    effect.typeEffect = TypeEffect.Heal;
                    effect.level = Random.Range(6, 15);
                }
                else
                {
                    effect.typeEffect = TypeEffect.Regen;
                }
            } else if (isDamage || (isForSupportExtraEffectSpell && isOffensiveBuff))
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

            effectSaved = effect;
            return effect;
        }

        private static string GetModelFromParameters()
        {
            string model = null;

            if (isAnArea)
            {
                if (isDamage)
                {
                    if (effectSaved != null)
                    {
                        model = effectSaved.typeEffect switch
                        {
                            TypeEffect.Burn => "Spells/Generation/FX_Auto_Zone_Burn1",
                            TypeEffect.Poison => "Spells/Generation/FX_Auto_Zone_Poison1",
                            TypeEffect.Freezing => "Spells/Generation/FX_Auto_Zone_Freeze1",
                            _ => "Spells/Generation/BasicArea"
                        };
                    }
                }

                if (isHeal)
                {
                    model = "Spells/Generation/FX_Healing_Cirle_02";
                }

                if (String.IsNullOrEmpty(model))
                {
                    model = "Spells/Generation/BasicArea";
                }
            } else if (isDistance)
            {
                if (isDamage)
                {
                    if (effectSaved != null)
                    {
                        model = effectSaved.typeEffect switch
                        {
                            TypeEffect.Burn => "Spells/Generation/FX_Auto_Burn" + Random.Range(1, 3),
                            TypeEffect.Poison => "Spells/Generation/FX_Auto_Poison" + Random.Range(1, 3),
                            TypeEffect.Freezing => "Spells/Generation/FX_Auto_Freeze" + Random.Range(1, 3),
                            _ => "Spells/DirtSlowFrog"
                        };
                    }
                }

                if (isHeal)
                {
                    model = target == StartFrom.TargetEntity ? "Spells/Generation/FX_Heal_02" : "Spells/Generation/FX_HealProjectile";
                }

                if (String.IsNullOrEmpty(model))
                {
                    model = "Spells/DirtSlowFrog";
                }
            }
            else if (isCac)
            {
                if (isAnArea)
                {
                    if (isHeal)
                    {
                        model = "Spells/Generation/FX_Healing_Cirle_02";
                    }
                }
                else if (isHeal)
                {
                    model = "Spells/Generation/FX_Heal_02";
                }
                else if (isDamage)
                {
                    if (effectSaved != null)
                    {
                        model = effectSaved.typeEffect switch
                        {
                            TypeEffect.Burn => "Spells/Generation/FX_Slash_Large_Red",
                            TypeEffect.Poison => "Spells/Generation/FX_Slash_Large_Green",
                            TypeEffect.Freezing => "Spells/Generation/FX_Slash_Blue",
                            _ => "Spells/Generation/FX_Slash_Large_01 1"
                        };
                    }
                }

                if (isSupport && effectSaved != null)
                {
                    model = effectSaved.typeEffect switch
                    {
                        TypeEffect.SpeedUp => "Spells/Generation/FX_BlueBuff",
                        TypeEffect.DefenseUp => "Spells/Generation/FX_YellowBuff",
                        _ => "Spells/Generation/FX_RedBuff"
                    };

                    if (isExtraBuff)
                    {
                        if (isDefensiveBuff)
                        {
                            model = "Spells/Generation/FX_Heal_02";
                        }
                    }
                }

                if (String.IsNullOrEmpty(model))
                {
                    model = "Spells/Generation/FX_Slash_Large_01 1";
                }
            }

            return model;
        }
    }
}