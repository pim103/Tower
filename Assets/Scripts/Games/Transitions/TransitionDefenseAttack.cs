﻿using Scripts.Games.Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 3;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private GameController gameController;

        [SerializeField]
        private InitAttackPhase initAttackPhase;

        private int defenseTimer;
        private string defenseTimerText;

        private void Start()
        {
            defenseTimer = durationDefensePhase;
            defenseTimerText = "Defense finish in";
        }

        private IEnumerator WaitingEndDefense()
        {
            objectsInScene.waitingCanvasGameObject.SetActive(true);
            objectsInScene.waitingText.text = defenseTimerText;

            while(defenseTimer > 0)
            {
                objectsInScene.counterText.text = defenseTimer.ToString();
                yield return new WaitForSeconds(1);
                defenseTimer--;
            }

            defenseTimer = durationDefensePhase;
            objectsInScene.waitingCanvasGameObject.SetActive(false);

            initAttackPhase.StartAttackPhase();
        }

        public void StartDefenseCounter()
        {
            StartCoroutine(WaitingEndDefense());
        }
    }
}