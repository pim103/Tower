using System.Collections.Generic;
using UnityEngine;

namespace Games.Defenses
{
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
        public GameObject[] trapModels;

        public int rotation = 0;

        public void CopyBehavior(TrapBehavior newTrapBehavior)
        {
            mainType = newTrapBehavior.mainType;
            trapEffects = newTrapBehavior.trapEffects;
            trapModels[(int)mainType].SetActive(true);
        }
    }
}
