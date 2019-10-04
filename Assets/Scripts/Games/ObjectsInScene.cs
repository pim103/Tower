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
    }
}