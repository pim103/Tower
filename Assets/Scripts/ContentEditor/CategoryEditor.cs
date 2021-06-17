using System.Collections.Generic;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Weapons;
using Games.Players;
using UnityEditor;
using UnityEngine;

namespace ContentEditor
{
    public class CategoryEditor : IEditorInterface
    {
        public bool createNewCategory = false;

        private CategoryWeapon currentCategory;

        private Dictionary<CategoryWeapon, CreateOrSelectComponent<Spell>> attackSpellSelectors = new Dictionary<CategoryWeapon, CreateOrSelectComponent<Spell>>();

        public void DisplayHeaderContent()
        {
            
        }

        public void DisplayBodyContent()
        {
            if (createNewCategory)
            {
                DisplayNewCategoryForm();
            }
            else
            {
                DisplayCategoryFormList();
            }
        }

        public void DisplayFooterContent()
        {
            if (!createNewCategory && GUILayout.Button("Créer une nouvelle catégorie"))
            {
                createNewCategory = true;
            }
            
            if (createNewCategory && GUILayout.Button("Annuler"))
            {
                createNewCategory = false;
                currentCategory = null;
            }
        }

        private void DisplayNewCategoryForm()
        {
            currentCategory ??= new CategoryWeapon();
            
            DisplayOneCategoryForm(currentCategory);
        }

        private void DisplayCategoryFormList()
        {
            int offsetX = 5;
            int offsetY = 0;

            if (DataObject.CategoryWeaponList == null)
            {
                return;
            }
            
            foreach (CategoryWeapon categoryWeapon in DataObject.CategoryWeaponList.categories)
            {
                GUILayout.BeginArea(new Rect(offsetX, offsetY, 300, 500));

                offsetX += 305;

                if (offsetX > EditorConstant.WIDTH)
                {
                    offsetX = 0;
                    offsetY += 500;
                }

                DisplayOneCategoryForm(categoryWeapon);

                GUILayout.EndArea();
            }
        }

        private void DisplayOneCategoryForm(CategoryWeapon categoryWeapon)
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", categoryWeapon.id);
            EditorGUI.EndDisabledGroup();

            categoryWeapon.name = EditorGUILayout.TextField("Name", categoryWeapon.name);
            
            if (DictionaryManager.hasSpellsLoad)
            {
                if (!attackSpellSelectors.ContainsKey(categoryWeapon))
                {
                    attackSpellSelectors.Add(categoryWeapon, 
                        new CreateOrSelectComponent<Spell>(DataObject.SpellList.SpellInfos.ConvertAll(s => s.spell), categoryWeapon.spellAttack, "Attack spell", null));
                }

                categoryWeapon.spellAttack = attackSpellSelectors[categoryWeapon].DisplayOptions();
            }
            
            Color currentColor = GUI.color;
            GUI.color = Color.green;

            if (GUILayout.Button("Sauvegarder la catégorie"))
            {
                PrepareSaveRequest.SaveCategoryWeapon(categoryWeapon, createNewCategory);
                createNewCategory = false;
            }
            GUI.color = currentColor;

            EditorGUILayout.EndVertical();
        }
    }
}