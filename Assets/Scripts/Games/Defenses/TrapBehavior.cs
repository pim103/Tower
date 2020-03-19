using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehavior : MonoBehaviour
{
    public enum TrapType
    {
        Arrows,
        Spikes,
        EjectionPlate,
        RammingRail,
        BearTrap,
        Fan,
        SpikyPole,
        Mine
    }

    public enum AdditionalEffects
    {
        Poison,
        Burn,
        Freeze,
        Weakness,
        Stun
    }

    public TrapType mainType;
    public List<AdditionalEffects> trapEffects;
    
    [SerializeField]
    private GameObject[] trapModels;
    
    public void CopyBehavior(TrapBehavior newTrapBehavior)
    {
        mainType = newTrapBehavior.mainType;
        trapEffects = newTrapBehavior.trapEffects;
        trapModels[(int)mainType].SetActive(true);
    }
}
