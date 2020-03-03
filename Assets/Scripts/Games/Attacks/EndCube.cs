using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.Attacks
{
    public class EndCube : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}