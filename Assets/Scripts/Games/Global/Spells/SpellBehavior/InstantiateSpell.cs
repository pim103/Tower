using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
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

            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);
            genericSpellPrefab.transform.localScale = spellToInstantiate.scale;
            genericSpellPrefab.transform.position = startPosition;

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

        public static void DeactivateSpell(SpellComponent spellComponent)
        {
            SpellPrefabController spellPrefabController = spellComponent.spellPrefabController;

            if (spellPrefabController == null)
            {
                return;
            }

            if (spellPrefabController.childrenGameObject != null)
            {
                spellPrefabController.childrenGameObject.SetActive(false);
            }

            spellPrefabController.transform.position = Vector3.zero;
            spellPrefabController.transform.localScale = Vector3.zero;
            spellPrefabController.ClearValues();
        }
    }
}