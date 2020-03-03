using Scripts.Games.Transitions;
using System.Collections;
using UnityEngine;

namespace Games {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField]
        private ScriptsExposer se;

        public static int PlayerIndex;

        private bool idAssigned = false;

        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

        private IEnumerator CheckEndInit()
        {
            yield return new WaitForSeconds(0.1f);
            transitionMenuGame.WantToStartGame();
        }

        // =================================== BYPASS DEFENSE METHOD ================================

        private IEnumerator WaitForDataLoading()
        {
            while (se.dm.monsterList == null || se.dm.weaponList == null)
            {
                yield return new WaitForSeconds(0.5f);
            }

            se.initAttackPhase.StartAttackPhase();
        }

        private void ForceStartAttackPhase()
        {
            StartCoroutine(WaitForDataLoading());
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);
            PlayerIndex = 0;

            if(byPassDefense)
            {
                ForceStartAttackPhase();
                return;
            }

            transitionMenuGame.WantToStartGame();
        }

        // TODO : Control player's movement here and not in PlayerMovement
    }
}