using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.Attacks
{
    public class EndCube : MonoBehaviour
    {
        [SerializeField] 
        private ObjectsInScene objectsInScene;
        [SerializeField] 
        private InitDefense initDefense;

        private void DesactiveAllGameObject()
        {
            foreach (GameObject go in DataObject.objectInScene)
            {
                go.SetActive(false);
            }

            foreach (Monster monster in DataObject.monsterInScene)
            {
                monster.entityPrefab.gameObject.SetActive(false);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Cursor.lockState = CursorLockMode.None;

            if (initDefense.currentLevel < initDefense.maps.Length)
            {
                DesactiveAllGameObject();
                
                initDefense.defenseUIController.enabled = false;
                objectsInScene.containerAttack.SetActive(false);
                objectsInScene.containerDefense.SetActive(true);
                initDefense.Init();
            }
            else
            {
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}