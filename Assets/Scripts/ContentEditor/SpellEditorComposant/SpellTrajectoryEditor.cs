using System;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using PathCreation;
using PathCreation.Examples;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentEditor.SpellEditorComposant
{
    public class SpellTrajectoryEditor
    {
        private static Scene sceneOpened;
        private static PathCreator currentPathCreator;
        private static PathFollower currentPathFollower;
        
        public static void DisplaySpellComponentTrajectory(SpellComponent spellComponentEdited)
        {
            if (spellComponentEdited == null)
            {
                return;
            }

            if (spellComponentEdited.trajectory == null)
            {
                spellComponentEdited.trajectory = new Trajectory();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical();
            
            spellComponentEdited.trajectory.followCategory = (FollowCategory) EditorGUILayout.EnumPopup("Catégorie de follow", spellComponentEdited.trajectory.followCategory);
            
            EditorGUI.BeginChangeCheck();
            spellComponentEdited.trajectory.speed = EditorGUILayout.FloatField("Vitesse", spellComponentEdited.trajectory.speed);
            if (EditorGUI.EndChangeCheck() && currentPathFollower != null)
            {
                currentPathFollower.speed = spellComponentEdited.trajectory.speed;
            }

            if (GUILayout.Button("Editer la trajectoire", GUILayout.Height(50)))
            {
                sceneOpened = EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/PathSpellCreator.unity");

                if (sceneOpened != null)
                {
                    foreach (GameObject go in sceneOpened.GetRootGameObjects())
                    {
                        if (go.name == "SpellPath")
                        {
                            currentPathCreator = go.GetComponent<PathCreator>();
                            if (spellComponentEdited.trajectory.spellPath != null)
                            {
                                currentPathCreator.bezierPath = spellComponentEdited.trajectory.spellPath;
                            }
                        } else if (go.name == "SphereFollower")
                        {
                            currentPathFollower = go.GetComponent<PathFollower>();
                        }
                    }
                }
            }

            if (currentPathCreator != null)
            {
                spellComponentEdited.trajectory.endOfPathInstruction = (EndOfPathInstruction)EditorGUILayout.EnumPopup("At the end of traj",
                    spellComponentEdited.trajectory.endOfPathInstruction);
                
                spellComponentEdited.trajectory.disapearAtTheEndOfTrajectory = EditorGUILayout.Toggle(
                    "Disparait à la fin de la traj", spellComponentEdited.trajectory.disapearAtTheEndOfTrajectory);
                
                if (GUILayout.Button("Sauvegarder le chemin actuel"))
                {
                    spellComponentEdited.trajectory.spellPath = currentPathCreator.bezierPath;
                }   
            }

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
    }
}