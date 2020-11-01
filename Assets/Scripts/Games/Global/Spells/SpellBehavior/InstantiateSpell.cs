using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using PathCreation;
using UnityEngine;

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
            
            Vector3 newPosition = startPosition;
            newPosition.y = spellToInstantiate.height;

            // INIT SPELL PATH
            if (spellComponent.trajectory.spellPath != null)
            {
                GameObject spellPathCreator = ObjectPooler.SharedInstance.GetPooledObject(0);
                
                spellPathCreator.transform.position = newPosition;
                spellPathCreator.transform.rotation = spellComponent.caster.entityPrefab.transform.rotation;

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
                genericSpellPrefab.transform.localScale = spellToInstantiate.scale;
                genericSpellPrefab.transform.position = newPosition;

                // INIT OBJECT CHILD OF GENERIC SPELL
                GameObject prefabWanted = null;
                if (spellComponent.spellToInstantiate.idPoolObject != -1)
                {
                    prefabWanted = ObjectPooler.SharedInstance.GetPooledObject(spellComponent.spellToInstantiate.idPoolObject);
                    prefabWanted.transform.parent = genericSpellPrefab.transform;
                    prefabWanted.transform.localPosition = Vector3.zero;
                    prefabWanted.transform.localEulerAngles = Vector3.zero;
                    prefabWanted.transform.localScale = Vector3.one;
                    prefabWanted.SetActive(true);   
                }

                SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
                spellPrefabController.ActiveCollider(spellToInstantiate.geometry);
                spellPrefabController.SetValues(spellComponent.caster, spellComponent, prefabWanted);

                spellComponent.spellPrefabController = spellPrefabController;
                
                genericSpellPrefab.SetActive(true);
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