using System;
using System.Collections;
using Games.Global;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;

namespace Games.Defenses
{
public class ArrowTrapBehavior : MonoBehaviour
{
    private GameObject target;
    private bool followTarget;
    private Coroutine projTimerCoroutine;

    private Entity entity;

    private void Start()
    {
        entity = new Entity
        {
            entityPrefab = gameObject.AddComponent<EntityPrefab>(),
            BehaviorType = BehaviorType.Player,
            typeEntity = TypeEntity.MOB
        };
        entity.entityPrefab.entity = entity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target = other.gameObject;
            followTarget = true;
            projTimerCoroutine = StartCoroutine(ProjTimer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StopCoroutine(projTimerCoroutine);
            followTarget = false;
            target = null;
        }
    }

    private void Update()
    {
        if (followTarget)
        {
            transform.LookAt(target.transform.position+Vector3.up);
            //transform.Rotate(Vector3.right,-90);
        }
    }

    IEnumerator ProjTimer()
    {
        while (true)
        {
            //PoolProjectiles();
            SpellController.CastSpell(entity, SpellController.LoadSpellByName("TrapArrowSpell"), transform.position);
            yield return new WaitForSeconds(.5f);
        }
    }
}
}
