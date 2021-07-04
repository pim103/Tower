#if UNITY_EDITOR_64 || UNITY_EDITOR

using ContentEditor.UtilsEditor;
using Games.Global.Spells;
using Games.Global.Spells.SpellsGenerator;
using PathCreation;
using TestC;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentEditor
{
    [SerializeField]
    public class SpellGeneratorEditor : IEditorInterface
    {
        public bool isHeal;
        public bool isSupport;
        public bool isDamage;

        public bool isCac;
        public bool isDistance;

        public static Spell spellGenerated;

        [SerializeField]
        private static PathCreator currentPathCreator;

        public void DisplayHeaderContent()
        {
            
        }

        public void OpenTrajectorySceneMode()
        {
            Scene sceneOpened;

            if (!UtilEditor.IsTestScene())
            {
                sceneOpened = EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/TestSpell.unity");;
            }
            else
            {
                sceneOpened = SceneManager.GetActiveScene();
            }

            foreach (GameObject go in sceneOpened.GetRootGameObjects())
            {
                if (go.name == "SpellPath")
                {
                    currentPathCreator = go.GetComponent<PathCreator>();
                }
            }
        }

        int nbInput = 0;
        public void DisplayBodyContent()
        {
            Color basicColor = GUI.backgroundColor;
            
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (isHeal)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.grey;
            }
            if (GUILayout.Button("Heal", GUILayout.Width(150), GUILayout.Height(50)))
            {
                isHeal = true;
                isDamage = false;
                isSupport = false;
            }
            
            
            if (isSupport)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.grey;
            }
            if (GUILayout.Button("Support", GUILayout.Width(150), GUILayout.Height(50)))
            {
                isHeal = false;
                isDamage = false;
                isSupport = true;
            }
            
            if (isDamage)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.grey;
            }
            if (GUILayout.Button("Damage", GUILayout.Width(150), GUILayout.Height(50)))
            {
                isHeal = false;
                isDamage = true;
                isSupport = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (isDistance)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.grey;
            }
            if (GUILayout.Button("Distance", GUILayout.Width(150), GUILayout.Height(50)))
            {
                isDistance = true;
                isCac = false;
            }
            
            if (isCac)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.grey;
            }
            if (GUILayout.Button("Cac", GUILayout.Width(150), GUILayout.Height(50)))
            {
                isDistance = false;
                isCac = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();

            GUI.backgroundColor = basicColor;
            EditorGUILayout.Space();

            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Generate spell", GUILayout.Width(200), GUILayout.Height(75)))
            {
                nbInput++;
                // if (nbInput == 1)
                // {
                //     spellGenerated = SpellController.LoadSpellByName("Hold");
                // }
                // else
                // {
                //     spellGenerated = SpellController.LoadSpellByName("spellGenerated2");
                // }

                spellGenerated = SpellGenerator.GenerateSpellWithParameter(currentPathCreator, isHeal, isSupport, isDamage, isCac, isDistance);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (UtilEditor.IsTestScene() &&
                spellGenerated != null &&
                GUILayout.Button("Test Spell", GUILayout.Width(200), GUILayout.Height(75)))
            {
                SpellTestScene spellTestScene = UtilEditor.GetSpellTestSceneScriptFromSceneTest();
                
                spellTestScene.LoadSpell(spellGenerated);
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        public void DisplayFooterContent()
        {
            
        }
    }
}

#endif