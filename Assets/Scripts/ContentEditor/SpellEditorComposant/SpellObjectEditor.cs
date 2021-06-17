#if UNITY_EDITOR || UNITY_EDITOR_64

using System;
using ContentEditor.UtilsEditor;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
using Games.Players;
using PathCreation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ContentEditor.SpellEditorComposant
{
    [Serializable]
    public class SpellObjectEditor
    {
        public static GameObject spellComponentObject;
        
        public static bool hasTrajectory = false;

        [SerializeField]
        private static Scene sceneOpened;
        [SerializeField]
        private static PathCreator currentPathCreator;

        private static GameObject instantiateModel;

        private static SpellObjectParameters _spellObjectParameters;

        private static Player player;
        private static Monster monster;

        private class SpellObjectParameters
        {
            public Transform sphereTransform;
            public SpellPrefabController spellPrefabController;

            public SpellObjectParameters(GameObject go)
            {
                sphereTransform = go.transform;
                spellPrefabController = go.GetComponent<SpellPrefabController>();
            }
        }

        public static void OpenAndInitScene(SpellComponent spellComponentEdited)
        {
            sceneOpened = EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/PathSpellCreator.unity");

            player = new Player();
            monster = new Monster();
            

            if (sceneOpened != null)
            {
                foreach (GameObject go in sceneOpened.GetRootGameObjects())
                {
                    if (go.name == "Player")
                    {
                        PlayerPrefab playerPrefab = go.GetComponent<PlayerPrefab>();
                        player.SetPlayerPrefab(playerPrefab);
                        playerPrefab.target = monster;
                    }
                    if (go.name == "Monster")
                    {
                        MonsterPrefab monsterPrefab = go.GetComponent<MonsterPrefab>();
                        monster.entityPrefab = monsterPrefab;
                        monsterPrefab.target = player;
                    }
                    if (go.name == "SpellPath")
                    {
                        currentPathCreator = go.GetComponent<PathCreator>();
                        if (spellComponentEdited.trajectory != null && spellComponentEdited.trajectory.spellPath != null)
                        {
                            currentPathCreator.bezierPath = spellComponentEdited.trajectory.spellPath;
                        }
                    } else if (go.name == "SpellObject")
                    {
                        _spellObjectParameters = new SpellObjectParameters(go);
                    }
                }
            }
        }
        
        public static void DisplaySpellComponentObject(Spell spell, SpellComponent spellComponentEdited)
        {
            if (spellComponentEdited == null)
            {
                return;
            }
            
            if (spellComponentEdited.spellToInstantiate == null)
            {
                spellComponentEdited.spellToInstantiate = new SpellToInstantiate();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // OBJECT INSTANTIATE
            
            EditorGUI.BeginChangeCheck();
            spellComponentEdited.spellToInstantiate.geometry = (Geometry) EditorGUILayout.EnumPopup("Geométrie", spellComponentEdited.spellToInstantiate.geometry);

            spellComponentEdited.spellToInstantiate.scale = EditorGUILayout.Vector3Field("Scale de départ", spellComponentEdited.spellToInstantiate.scale);
            spellComponentEdited.spellToInstantiate.offsetStartPosition = EditorGUILayout.Vector3Field("Offset à la position de départ", 
                spellComponentEdited.spellToInstantiate.offsetStartPosition);

            EditorGUILayout.LabelField("Model");
            spellComponentObject = (GameObject)EditorGUILayout.ObjectField(spellComponentObject, typeof(GameObject), false);

            if (spellComponentObject)
            {
                spellComponentEdited.spellToInstantiate.offsetObjectToInstantiate = EditorGUILayout.Vector3Field("Offset de l'objet à instancier", 
                    spellComponentEdited.spellToInstantiate.offsetObjectToInstantiate);
            }

            if (!spellComponentObject && spellComponentEdited.spellToInstantiate.pathGameObjectToInstantiate != null)
            {
                spellComponentObject =
                    Resources.Load<GameObject>(spellComponentEdited.spellToInstantiate.pathGameObjectToInstantiate);
            }

            if (EditorGUI.EndChangeCheck())
            {
                StartFrom startFrom = StartFrom.Caster;
                
                if (spell != null)
                {
                    startFrom = spell.startFrom;
                }

                TargetsFound targetsFound = SpellController.GetTargetGetWithStartForm(player, startFrom);

                _spellObjectParameters.spellPrefabController.SetSpellParameter(spellComponentEdited, targetsFound.position, false);

                if (instantiateModel)
                {
                    Object.DestroyImmediate(instantiateModel);
                }

                if (spellComponentObject)
                {
                    spellComponentEdited.spellToInstantiate.pathGameObjectToInstantiate =
                        UtilEditor.GetObjectPathInRessourceFolder(spellComponentObject);

                    instantiateModel = Object.Instantiate(spellComponentObject, _spellObjectParameters.sphereTransform);
                    instantiateModel.transform.position +=
                        spellComponentEdited.spellToInstantiate.offsetObjectToInstantiate;
                }
                else
                {
                    spellComponentEdited.spellToInstantiate.pathGameObjectToInstantiate = null;
                }
            }

            // spellComponentEdited.spellToInstantiate.idPoolObject = EditorGUILayout.IntField("Objet à pool", spellComponentEdited.spellToInstantiate.idPoolObject);
            spellComponentEdited.spellToInstantiate.incrementAmplitudeByTime = EditorGUILayout.Vector3Field("Increment du scale", spellComponentEdited.spellToInstantiate.incrementAmplitudeByTime);
            spellComponentEdited.spellToInstantiate.passingThroughEntity = EditorGUILayout.Toggle("Traverse les entités", spellComponentEdited.spellToInstantiate.passingThroughEntity);
            
            // TRAJECTORY PARAMETERS
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            hasTrajectory = EditorGUILayout.Toggle("Possède une trajectorie", hasTrajectory) || spellComponentEdited.trajectory != null;

            if (hasTrajectory)
            {
                if (spellComponentEdited.trajectory == null)
                {
                    spellComponentEdited.trajectory = new Trajectory();
                }
                
                spellComponentEdited.trajectory.followCategory = (FollowCategory) EditorGUILayout.EnumPopup("Catégorie de follow", spellComponentEdited.trajectory.followCategory);
                spellComponentEdited.trajectory.speed = EditorGUILayout.FloatField("Vitesse", spellComponentEdited.trajectory.speed);
 
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
            }
            else
            {
                spellComponentEdited.trajectory = null;
            }

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
    }
}

#endif