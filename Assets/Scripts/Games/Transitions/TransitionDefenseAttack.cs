using System.Collections;
using Games.Attacks;
using Games.Defenses;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Transitions
{
    public class TransitionDefenseAttack : MonoBehaviour
    {
        private const int durationDefensePhase = 300;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private GameController gameController;
        
        [SerializeField]
        private Text counter;

        [SerializeField]
        private InitAttackPhase initAttackPhase;
        
        [SerializeField]
        private InitDefense initDefense;

        private int defenseTimer;

        private void Start()
        {
            defenseTimer = durationDefensePhase;
        }

        private IEnumerator WaitingEndDefense()
        {
            objectsInScene.waitingCanvasGameObject.SetActive(true);
            objectsInScene.waitingText.text = "";
            objectsInScene.counterText.text = "";
            while(defenseTimer > 0)
            {
                counter.text = defenseTimer.ToString();
                yield return new WaitForSeconds(1);
                defenseTimer--;
            }

            defenseTimer = durationDefensePhase;
            objectsInScene.waitingCanvasGameObject.SetActive(false);

            SendGridData();
            initAttackPhase.StartAttackPhase();
        }

        public void StartDefenseCounter()
        {
            StartCoroutine(WaitingEndDefense());
        }

        private void SendGridData()
        {
            string stringToSend = "{\n";
            foreach (var gridCell in initDefense.gridCellList)
            {
                GridTileController cellController = gridCell.GetComponent<GridTileController>();
                if (cellController.content)
                {
                    if (cellController.content.layer == LayerMask.NameToLayer("Group"))
                    {
                        //stringToSend += "1:" + "manqueId:"+cellController.content.GetComponent<MonsterPrefab>().hand.transform.GetChild(0).;
                    } else if (cellController.content.layer == LayerMask.NameToLayer("Wall"))
                    {
                        
                    } else if (cellController.content.layer == LayerMask.NameToLayer("Trap"))
                    {
                        
                    }
                }
            }
        }
    }
}