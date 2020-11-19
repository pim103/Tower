using System.Collections.Generic;
using Games.Defenses.Traps;
using UnityEngine;

namespace Games.Defenses
{
    public class TrapBehavior : MonoBehaviour
    {
        [SerializeField]
        public GameObject[] trapModels;

        public int rotation = 0;

        public TrapData trapData;

        public void SetAndActiveTraps(TrapData trapData)
        {
            this.trapData = trapData;
            trapModels[(int)trapData.mainType].SetActive(true);
        }
    }
}
