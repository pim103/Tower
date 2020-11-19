using System;
using UnityEngine;

namespace Games
{
    public class ContainerController : MonoBehaviour
    {
        private static ContainerController instance;

        [SerializeField] private GameObject mainCamera;
        [SerializeField] private GameObject selectCharMenu;
        [SerializeField] private GameObject defenseContainer;
        [SerializeField] private GameObject attackContainer;
        
        private void Start()
        {
            instance = this;
        }

        private static void DeactiveAllObjects()
        {
            instance.mainCamera.SetActive(false);
            instance.selectCharMenu.SetActive(false);
            instance.defenseContainer.SetActive(false);
            instance.attackContainer.SetActive(false);
        }

        public static void ActiveContainerOfCurrentPhase(Phase phase)
        {
            DeactiveAllObjects();
            
            switch (phase)
            {
                case Phase.RoleAndDeck:
                    instance.mainCamera.SetActive(true);
                    instance.selectCharMenu.SetActive(true);
                    break;
                case Phase.Defense:
                    instance.defenseContainer.SetActive(true);
                    break;
                case Phase.Attack:
                    instance.attackContainer.SetActive(true);
                    break;
            }
        }
    }
}