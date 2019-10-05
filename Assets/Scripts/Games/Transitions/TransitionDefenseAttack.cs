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

            StartAttackPhase();
        }

        public void StartDefenseCounter()
        {
            StartCoroutine(WaitingEndDefense());
        }

        public void StartAttackPhase()
        {
            objectsInScene.containerDefense.SetActive(false);
            objectsInScene.containerAttack.SetActive(true);

            objectsInScene.playersCamera[gameController.PlayerIndex].SetActive(true);

            for (int i = 0; i < objectsInScene.playersMovement.Length; i++)
            {
                if(gameController.idToUserId[i] != null && gameController.idToUserId[i] != "")
                {
                    objectsInScene.playersGameObject[i].SetActive(true);
                    objectsInScene.playersMovement[i].canMove = true;
                }
            }
        }
    }
}