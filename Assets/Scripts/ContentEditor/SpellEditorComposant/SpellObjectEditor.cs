#if UNITY_EDITOR || UNITY_EDITOR_64

using System;
using ContentEditor.UtilsEditor;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEditor;
using UnityEngine;

namespace ContentEditor.SpellEditorComposant
{
    [Serializable]
    public class SpellObjectEditor
    {
        public static GameObject spellComponentObject;
        
        public static void DisplaySpellComponentObject(SpellComponent spellComponentEdited)
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
            EditorGUILayout.BeginVertical();
            
            spellComponentEdited.spellToInstantiate.geometry = (Geometry) EditorGUILayout.EnumPopup("Geométrie", spellComponentEdited.spellToInstantiate.geometry);
            spellComponentEdited.spellToInstantiate.scale = EditorGUILayout.Vector3Field("Scale de départ", spellComponentEdited.spellToInstantiate.scale);
            spellComponentEdited.spellToInstantiate.height = EditorGUILayout.FloatField("Hauteur de départ", spellComponentEdited.spellToInstantiate.height);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Model");
            spellComponentObject = (GameObject)EditorGUILayout.ObjectField(spellComponentObject, typeof(GameObject), false);

            if (EditorGUI.EndChangeCheck() && spellComponentObject)
            {
                spellComponentEdited.spellToInstantiate.pathGameObjectToInstantiate = UtilEditor.GetObjectPathInRessourceFolder(spellComponentObject);
            }
            
            // spellComponentEdited.spellToInstantiate.idPoolObject = EditorGUILayout.IntField("Objet à pool", spellComponentEdited.spellToInstantiate.idPoolObject);
            spellComponentEdited.spellToInstantiate.incrementAmplitudeByTime = EditorGUILayout.Vector3Field("Increment du scale", spellComponentEdited.spellToInstantiate.incrementAmplitudeByTime);
            spellComponentEdited.spellToInstantiate.passingThroughEntity = EditorGUILayout.Toggle("Traverse les entités", spellComponentEdited.spellToInstantiate.passingThroughEntity);
            
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
    }
}

#endif