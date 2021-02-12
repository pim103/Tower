using System;
using Games.Players;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
        
        public static string GetObjectInRessourcePath(Object objectToFindPath)
        {
            string spritePath = AssetDatabase.GetAssetPath(objectToFindPath);
            const string resourcesFolder = "Resources/";

            if (spritePath.Contains(resourcesFolder))
            {
                int indexOfResources = spritePath.IndexOf(resourcesFolder, StringComparison.CurrentCulture);
                spritePath = spritePath.Substring(indexOfResources + resourcesFolder.Length);

                int indexOfExtension = spritePath.IndexOf(".", StringComparison.CurrentCulture);
                if (indexOfExtension != -1)
                {
                    spritePath = spritePath.Substring(0, indexOfExtension);
                }
            }
            else
            {
                Debug.Log("L'image n'est pas dans Resource !");
                spritePath = "";
            }

            return spritePath;
        }
    }
}