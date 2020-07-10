using System;
using System.Collections;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;

namespace Games.Defenses
{
public class SpikesBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject spikes;
    private bool isActive;
    private bool playerOnSpikes;

    private Entity playerEntity;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            playerOnSpikes = true;
            playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            playerOnSpikes = false;
            playerEntity = null;
        }
    }

    private void Update()
    {
        if (playerOnSpikes && !isActive)
        {
            StartCoroutine(GoUp());
        }
    }

    private IEnumerator GoUp()
    {
        isActive = true;
        int trapTimer = 0;
        var spikesPosition = spikes.transform.position;
        spikesPosition = new Vector3(spikesPosition.x,spikesPosition.y+1,spikesPosition.z);
        spikes.transform.position = spikesPosition;
        AbilityParameters abilityParameters = new AbilityParameters();
        abilityParameters.origin = entity;
        playerEntity.TakeDamage(10.0f, abilityParameters, DamageType.Physical);
        while (trapTimer<3)
        {
            yield return new WaitForSeconds(1f);
            trapTimer += 1;
        }
        spikesPosition = spikes.transform.position;
        spikesPosition = new Vector3(spikesPosition.x,spikesPosition.y-1,spikesPosition.z);
        spikes.transform.position = spikesPosition;
        yield return new WaitForSeconds(1f);
        isActive = false;
    }
}
}
