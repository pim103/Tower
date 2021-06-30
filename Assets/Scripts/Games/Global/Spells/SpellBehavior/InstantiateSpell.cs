using System;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using PathCreation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Games.Global.Spells.SpellBehavior
{
    public class InstantiateSpell
    {
        public static void InstantiateNewSpell(SpellComponent spellComponent, Vector3 startPosition)
        {
            SpellToInstantiate spellToInstantiate = spellComponent.spellToInstantiate;

            if (spellToInstantiate == null)
            {
                return;
            }

            PathCreator pathCreator = null;
            // INIT SPELL PATH
            if (spellComponent.trajectory != null && spellComponent.trajectory.spellPath != null)
            {
                GameObject spellPathCreator = ObjectPooler.SharedInstance.GetPooledObject(0);
                
                Vector3 offset = spellComponent.spellToInstantiate.offsetStartPosition;
                Vector3 forward = spellComponent.caster.entityPrefab.transform.forward;
                Vector3 position = startPosition;

                position += forward * offset.z + forward * offset.x +
                            Vector3.up * offset.y;
            
                spellPathCreator.transform.position = position;
                spellPathCreator.transform.forward = forward;

                spellPathCreator.SetActive(true);

                pathCreator = spellPathCreator.GetComponent<PathCreator>();
                pathCreator.bezierPath = spellComponent.trajectory.spellPath;

                spellComponent.trajectory.initialParent = spellPathCreator.transform.parent;
                if (spellComponent.trajectory.objectToFollow != null)
                {
                    spellPathCreator.transform.parent = spellComponent.trajectory.objectToFollow;
                }
            }

            spellComponent.pathCreator = pathCreator;

            if (spellComponent.spellToInstantiate != null)
            {
                // INIT GENERIC SPELL PREFAB
                GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);
                genericSpellPrefab.SetActive(true);
                SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();

                spellPrefabController.SetSpellParameter(spellComponent, startPosition);

                spellComponent.spellPrefabController = spellPrefabController;
            }
        }

        public static void DeactivateSpell(SpellComponent spellComponent)
        {
            SpellPrefabController spellPrefabController = spellComponent.spellPrefabController;

            if (spellComponent.pathCreator != null)
            {
                spellComponent.pathCreator.transform.parent = spellComponent.trajectory.initialParent;
                spellComponent.pathCreator.gameObject.SetActive(false);
            }

            if (spellPrefabController != null)
            {
                if (spellPrefabController.childrenGameObject != null)
                {
                    spellPrefabController.childrenGameObject.SetActive(false);
                }

                spellPrefabController.transform.position = Vector3.zero;
                spellPrefabController.transform.localScale = Vector3.zero;
                spellPrefabController.ClearValues();

                spellPrefabController.gameObject.SetActive(false);
            }
        }
    }
}