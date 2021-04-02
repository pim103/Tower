using System.Collections.Generic;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Weapons;
using Games.Players;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public class PlayerEditor : IEditorInterface
    {
        public bool createNewClasses = false;
        public Dictionary<int, Classes> ClassesMap = new Dictionary<int, Classes>();

        private Classes currentClasses;

        public void DisplayHeaderContent()
        {
            
        }

        public void DisplayBodyContent()
        {
            if (createNewClasses)
            {
                DisplayNewClassesForm();
            }
            else
            {
                DisplayClassesFormList();
            }
        }

        public void DisplayFooterContent()
        {
            
        }

        private void DisplayNewClassesForm()
        {
            currentClasses ??= new Classes();
            
            DisplayOneClassesForm(currentClasses);
        }

        private void DisplayClassesFormList()
        {
            int offsetX = 5;
            int offsetY = 0;
            
            foreach (KeyValuePair<int, Classes> classes in ClassesMap)
            {
                GUILayout.BeginArea(new Rect(offsetX, offsetY, 300, 500));

                offsetX += 305;

                if (offsetX > EditorConstant.WIDTH)
                {
                    offsetX = 0;
                    offsetY += 500;
                }
                
                DisplayOneClassesForm(classes.Value);

                GUILayout.EndArea();
            }
        }

        private void DisplayOneClassesForm(Classes classes)
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            // GUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            // GUILayout.Button(weapon.sprite, GUILayout.Width(75), GUILayout.Height(75));
            // GUILayout.FlexibleSpace();
            // GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", classes.id);
            EditorGUI.EndDisabledGroup();

            classes.name = EditorGUILayout.TextField("Name", classes.name);
            classes.hp = EditorGUILayout.IntField("Hp", classes.hp);
            classes.att = EditorGUILayout.IntField("Attack", classes.att);
            classes.def = EditorGUILayout.IntField("Defense", classes.def);
            classes.attSpeed = EditorGUILayout.IntField("Attack speed", classes.attSpeed);
            classes.ressource = EditorGUILayout.IntField("Ressource", classes.ressource);
            classes.speed = EditorGUILayout.IntField("Speed", classes.speed);
            
            EditorGUI.BeginDisabledGroup(true);
            classes.defenseSpell = EditorGUILayout.TextField("Defense spell (soon)", classes.defenseSpell);
            EditorGUI.EndDisabledGroup();
            
            // EditorGUILayout.LabelField("Model");
            // weapon.model = (GameObject)EditorGUILayout.ObjectField(weapon.model, typeof(GameObject), false);

            // EditorGUILayout.LabelField("Sprite");
            // weapon.sprite = (Texture2D)EditorGUILayout.ObjectField(weapon.sprite, typeof(Texture2D), false);

            if (GUILayout.Button("Play Classes") && UtilEditor.IsTestScene())
            {
                Player player = UtilEditor.GetPlayerFromSceneTest();

                if (player == null)
                {
                    Debug.LogError("Can't find player object in current scene");
                }
                else
                {
                    player.InitClasses(currentClasses);
                }
            }

            EditorGUILayout.EndVertical();
        }

        public void CloneOriginalClasses()
        {
            ClassesMap.Clear();

            foreach (Classes classes in DataObject.ClassesList.classes)
            {
                ClassesMap.Add(classes.id, Tools.Clone(classes));
            }
        }
    }
}