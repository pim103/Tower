using System;
using System.Collections.Generic;
using System.Drawing;
using Games.Global.Spells.SpellParameter;
using PathCreation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.Global.Spells.SpellsGenerator
{
    public class SpellGenerator
    {
        public static Spell GenerateSpellWithParameter(
            PathCreator path,
            bool isHeal,
            bool isSupport,
            bool isDamage,
            bool isCac,
            bool isDistance)
        {
            SpellComponent activeSpellComponent = GenerateActiveSpellComponent(path, isHeal, isSupport, isDamage, isCac, isDistance);
            
            Spell spell = new Spell
            {
                activeSpellComponent = activeSpellComponent,
                cooldown = Random.Range(1, 4),
                cost = Random.Range(1, 4),
                castTime = 0,
                nameSpell = "SpellGenerateAt_" + DateTime.Now,
                startFrom = StartFrom.Caster,
            };

            return spell;
        }

        private static SpellComponent GenerateActiveSpellComponent(
            PathCreator path,
            bool isHeal,
            bool isSupport,
            bool isDamage,
            bool isCac,
            bool isDistance)
        {
            Trajectory trajectory = null;
            if (isDistance)
            {
                trajectory = GenerateTrajectory(path);
            }

            SpellToInstantiate spellToInstantiate = GenerateSpellToInstantiate(isDistance);

            Dictionary<Trigger, List<ActionTriggered>> actions = GenerateSpellAction(isHeal, isSupport, isDamage);

            SpellComponent spellComponent = new SpellComponent
            {
                actions = actions,
                trajectory = trajectory,
                spellToInstantiate = spellToInstantiate,
            };

            return spellComponent;
        }
        
        private static Trajectory GenerateTrajectory(PathCreator path)
        {
            path.bezierPath.SetPoint(1, Vector3.forward * Random.Range(10, 20));

            Trajectory trajectory = new Trajectory
            {
                followCategory = FollowCategory.NONE,
                speed = Random.Range(5, 10),
                spellPath = path.bezierPath,
                endOfPathInstruction = EndOfPathInstruction.Stop,
                disapearAtTheEndOfTrajectory = true
            };

            return trajectory;
        }

        private static SpellToInstantiate GenerateSpellToInstantiate(bool isDistance)
        {
            int scaleX = isDistance ? Random.Range(1, 5) : Random.Range(2, 7);
            int scaleZ = isDistance ? Random.Range(1, 5) : Random.Range(4, 10);
            
            return new SpellToInstantiate
            {
                geometry = (Geometry) Random.Range(0, 3),
                height = 1,
                scale = new Vector3(scaleX, 1, scaleZ),
                passingThroughEntity = false
            };
        }

        private static Dictionary<Trigger, List<ActionTriggered>> GenerateSpellAction(bool isHeal, bool isSupport, bool isDamage)
        {
            Trigger trigger = Trigger.ON_TRIGGER_ENTER;
            int damage = Random.Range(25, 55);

            ActionTriggered actionTriggered = new ActionTriggered
            {
                damageDeal = damage,
                startFrom = StartFrom.AllEnemiesInArea,
            };

            Dictionary<Trigger, List<ActionTriggered>> actions = new Dictionary<Trigger, List<ActionTriggered>>();
            actions.Add(trigger, new List<ActionTriggered>{ actionTriggered });

            return actions;
        }
    }
}