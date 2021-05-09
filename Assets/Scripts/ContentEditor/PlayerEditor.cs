using System.Collections.Generic;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Spells;
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

        private Dictionary<Classes, CreateOrSelectComponent<Spell>> defensesSpellSelector = new Dictionary<Classes, CreateOrSelectComponent<Spell>>();

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
            if (!createNewClasses && GUILayout.Button("Créer une nouvelle classe"))
            {
                createNewClasses = true;
            }
            
            if (createNewClasses && GUILayout.Button("Annuler"))
            {
                createNewClasses = false;
                currentClasses = null;
            }
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
            
            if (DictionaryManager.hasSpellsLoad)
            {
                if (!defensesSpellSelector.ContainsKey(classes))
                {
                    defensesSpellSelector.Add(classes, new CreateOrSelectComponent<Spell>(DataObject.SpellList.SpellInfos.ConvertAll(s => s.spell), classes.defenseSpell, "Defense spell", null));
                }

                classes.defenseSpell = defensesSpellSelector[classes].DisplayOptions();
            }

            Color currentColor = GUI.color;
            GUI.color = Color.green;

            if (GUILayout.Button("Sauvegarder la classe"))
            {
                PrepareSaveRequest.SaveClasses(classes, createNewClasses);
                createNewClasses = false;
            }
            GUI.color = currentColor;

            if (GUILayout.Button("Play Classes") && UtilEditor.IsTestScene())
            {
                Player player = UtilEditor.GetPlayerFromSceneTest();

                if (player == null)
                {
                    Debug.LogError("Can't find player object in current scene");
                }
                else
                {
                    player.InitClasses(classes);
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