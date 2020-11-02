using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEditor;
using UnityEngine;

namespace ContentEditor.SpellEditorComposant
{
    public class SpellTrajectoryEditor
    {
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
            spellComponentEdited.trajectory.speed = EditorGUILayout.FloatField("Vitesse", spellComponentEdited.trajectory.speed);

            // TODO : Implement drawing bezier curve
            if (spellComponentEdited.trajectory.spellPath != null)
            {
                spellComponentEdited.trajectory.followCategory = (FollowCategory) EditorGUILayout.EnumPopup("Geométrie", spellComponentEdited.trajectory.followCategory);
                spellComponentEdited.trajectory.disapearAtTheEndOfTrajectory = EditorGUILayout.Toggle(
                    "Disparait à la fin de la traj", spellComponentEdited.trajectory.disapearAtTheEndOfTrajectory);
            }

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();
        }
    }
}