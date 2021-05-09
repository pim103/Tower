using System.Collections.Generic;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Spells;
using Games.Global.TreeBehavior.CompositeBehavior;
using Games.Global.Weapons;
using Games.Players;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public class PlayerEditor : IEditorInterface
    {
        private Classes editedClasses;
        public bool createNewClasses = false;
        public bool editSpell = false;

        private List<ClassesWeaponSpell> classesWeaponSpells = new List<ClassesWeaponSpell>();
        private Dictionary<ClassesWeaponSpell, List<CreateOrSelectComponent<Spell>>> classesWeaponSpellSelector = new Dictionary<ClassesWeaponSpell, List<CreateOrSelectComponent<Spell>>>();
        
        public Dictionary<int, Classes> ClassesMap = new Dictionary<int, Classes>();
        private Classes currentClasses;
        private Dictionary<Classes, CreateOrSelectComponent<Spell>> defensesSpellSelector = new Dictionary<Classes, CreateOrSelectComponent<Spell>>();

        public void DisplayHeaderContent()
        {
            
        }

        public void DisplayBodyContent()
        {
            if (editSpell)
            {
                DisplaySpellClasses();
            }
            else if (createNewClasses)
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
            if (editSpell)
            {
                if (GUILayout.Button("Annuler"))
                {
                    editSpell = false;
                }
                return;
            }
            
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

        private void DisplaySpellClasses()
        {
            int offsetX = 5;
            int offsetY = 0;

            foreach (ClassesWeaponSpell classesWeaponSpell in classesWeaponSpells)
            {
                GUILayout.BeginArea(new Rect(offsetX, offsetY, 300, 500));

                offsetX += 305;

                if (offsetX > EditorConstant.WIDTH)
                {
                    offsetX = 0;
                    offsetY += 500;
                }
                
                DisplayOneCategorySpells(classesWeaponSpell);

                GUILayout.EndArea();
            }
        }

        private void DisplayOneCategorySpells(ClassesWeaponSpell spellForClasses)
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Classes name", spellForClasses.classes.name);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Category", spellForClasses.categoryWeapon.name);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("id", spellForClasses.id);
            EditorGUI.EndDisabledGroup();

            spellForClasses.spell1 = classesWeaponSpellSelector[spellForClasses][0].DisplayOptions();
            spellForClasses.spell2 = classesWeaponSpellSelector[spellForClasses][1].DisplayOptions();
            spellForClasses.spell3 = classesWeaponSpellSelector[spellForClasses][2].DisplayOptions();
            
            Color currentColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("Sauvegarder les spells"))
            {
                if (createNewClasses)
                {
                    PrepareSaveRequest.SaveClasses(currentClasses, createNewClasses);
                    createNewClasses = false;
                }
                
                PrepareSaveRequest.SaveSpellForClasses(spellForClasses, spellForClasses.id == 0);
                editSpell = false;
            }
            GUI.color = currentColor;

            EditorGUILayout.EndVertical();
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

            if (GUILayout.Button("Editer les spells"))
            {
                editedClasses = classes;
                editSpell = true;
                classesWeaponSpells.Clear();
                classesWeaponSpellSelector.Clear();
                InitSpellForClasses();
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

        private void InitSpellForClasses()
        {
            List<ClassesWeaponSpell> existings = DataObject.ClassesList.GetSpellCategoryForClasses(editedClasses);
            List<Spell> existingSpell = DataObject.SpellList.SpellInfos.ConvertAll(s => s.spell);
            
            foreach (CategoryWeapon categoryWeapon in DataObject.CategoryWeaponList.categories)
            {
                List<CreateOrSelectComponent<Spell>> selectors = new List<CreateOrSelectComponent<Spell>>();

                ClassesWeaponSpell existing = existings.Find(data => data.categoryWeapon.id == categoryWeapon.id) ?? new ClassesWeaponSpell
                {
                    categoryWeapon = categoryWeapon,
                    classes = editedClasses
                };

                selectors.Add(new CreateOrSelectComponent<Spell>(existingSpell, existing.spell1, "Spell 1", null));
                selectors.Add(new CreateOrSelectComponent<Spell>(existingSpell, existing.spell2, "Spell 2", null));
                selectors.Add(new CreateOrSelectComponent<Spell>(existingSpell, existing.spell3, "Spell 3", null));

                classesWeaponSpellSelector.Add(existing, selectors);
                classesWeaponSpells.Add(existing);
            }
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