using Scripts;
using Scripts.Games.Attacks;
using Scripts.Games.Global;
using Scripts.Games.Global.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games
{
    public class ScriptsExposer : MonoBehaviour
    {
        [SerializeField]
        public PhotonController photonController;
        
        [SerializeField]
        public InitAttackPhase initAttackPhase;

        [SerializeField]
        public WeaponList weaponList;

        [SerializeField]
        public GameController gameController;
    }
}
