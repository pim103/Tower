using System;
using Games.Global.Weapons;
using Games.Players;
using TestC;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
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

        public static SpellTestScene GetSpellTestSceneScriptFromSceneTest()
        {
            SpellTestScene spellTestScene = null;
            
            GameObject go = GameObject.Find("TestController");

            if (go)
            {
                spellTestScene = go.GetComponent<SpellTestScene>();
            }

            return spellTestScene;
        }
        
        public static string GetObjectPathInRessourceFolder(Object objectToFindPath)
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