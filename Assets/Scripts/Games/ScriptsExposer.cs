using Scripts;
using Scripts.Games.Attacks;
using Scripts.Games.Global;
using System.Collections;
using System.Collections.Generic;
using Games.Attacks;
using Games.Global.Weapons;
using Scripts.Games;
using UnityEngine;

namespace Games
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
