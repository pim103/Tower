using Games.Defenses;
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
        private void OnTriggerEnter(Collider other)
        {
            Cursor.lockState = CursorLockMode.None;

            if (initDefense.currentLevel < initDefense.maps.Length)
            {
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