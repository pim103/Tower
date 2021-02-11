using Games.Players;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentEditor.UtilsEditor
{
    public class UtilEditor
    {
        public const string TestSceneName = "TestSpell";

        public static bool IsTestScene()
        {
            return SceneManager.GetActiveScene().name.Equals(TestSceneName);
        }

        public static Player GetPlayerFromSceneTest()
        {
            Player player = null;
            
            GameObject playerGo = GameObject.Find("Player");

            if (playerGo != null)
            {
                player = playerGo.GetComponent<PlayerPrefab>()?.entity as Player;
            }

            return player;
        }
    }
}