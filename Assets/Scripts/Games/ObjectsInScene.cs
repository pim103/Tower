using Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Games
{
    public class ObjectsInScene : MonoBehaviour
    {
        // ============================ MAIN CAMERA ========================

        [SerializeField]
        public GameObject mainCamera;

        // ============================ CANVAS AND COUNTER ACCESS =============

        [SerializeField]
        public GameObject waitingCanvasGameObject;

        [SerializeField]
        public Text waitingText;

        [SerializeField]
        public Text counterText;

        // ============================ PRINCIPAL ATTACK AND DEFENSE CONTAINER =============

        [SerializeField]
        public GameObject containerDefense;

        [SerializeField]
        public GameObject containerAttack;

        // =========================== PLAYER CHARACTER ========================

        [SerializeField]
        public GameObject[] playersGameObject;

        [SerializeField]
        public PlayerMovement[] playersMovement;

        [SerializeField]
        public Rigidbody[] playersRigidbody;

        [SerializeField]
        public GameObject[] playersCamera;

        // ========================== ACCESS TO MAP CONTAINER ==================

        [SerializeField]
        public GameObject[] maps;

        // ========================== OBJECT GENERATE IN MAP ==================

        [SerializeField]
        public GameObject simpleWall;
    }
}