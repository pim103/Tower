using UnityEngine;
using UnityEditor;

namespace InfinityPBR
{
    [CustomEditor(typeof(ColorShifterObject))]
    public class ColorShifterObjectEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("To edit this Color Shifter Object, select \"Window/Infinity PBR/Color Shifter\", and add this object to the appropriate field in that Editor window.",
                MessageType.Warning);
            
            DrawDefaultInspector();
        }
    }
}
