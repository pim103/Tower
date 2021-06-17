#if UNITY_EDITOR_64 || UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ContentEditor.UtilsEditor
{
    public class OptionSelected<T>
    {
        public T option;
    }

    public class CreateOrSelectComponent<T>
    {
        private bool displayChoice;
        private readonly SingleSelectList<T> singleSelectList;

        private T currentChoiceSelected;
        private readonly string label;

        private readonly Action<CreateOrSelectComponent<T>> onCreateAction;
        
        public CreateOrSelectComponent(
            List<T> spellComponents,
            T currentChoice,
            string newLabel,
            Action<CreateOrSelectComponent<T>> onCreate)
        {
            singleSelectList = new SingleSelectList<T>(spellComponents);
            currentChoiceSelected = currentChoice;
            label = newLabel;
            onCreateAction = onCreate;
        }

        public void ResetChoice()
        {
            displayChoice = false;
        }
        
        public T DisplayOptions()
        {
            Color defaultColor = GUI.color;
            GUI.color = Color.cyan;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            EditorGUILayout.LabelField(label + " ");
            // Affichage des différents spell component dans la liste
            if (displayChoice)
            {
                OptionSelected<T> option = singleSelectList.DisplaySpellComponentOption();

                if (option != null)
                {
                    currentChoiceSelected = option.option;
                    displayChoice = false;
                    
                    if (option.option == null)
                    {
                        displayChoice = false;
                    }
                }
            } 
            else if (currentChoiceSelected != null)
            {
                // Affichage si un spell component est choisi
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(currentChoiceSelected.ToString());
                if (GUILayout.Button("Supprimer le component"))
                {
                    currentChoiceSelected = default(T);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                // Affichage si aucun spellComponent n'est sélectionné
                if (onCreateAction != null && GUILayout.Button("Créer un composant"))
                {
                    onCreateAction(this);
                }

                if (GUILayout.Button("Choisir un composant"))
                {
                    displayChoice = true;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            return currentChoiceSelected;
        }
    }
    
    public class SingleSelectList<T>
    {
        private readonly List<T> choices;
        
        public SingleSelectList(List<T> choiceList)
        {
            choices = choiceList;
        }

        public OptionSelected<T> DisplaySpellComponentOption()
        {
            OptionSelected<T> selectedChoice = null;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (GUILayout.Button("Aucun", GUILayout.Height(50)))
            {
                selectedChoice = new OptionSelected<T> {option = default(T)};
            }
            foreach (T choice in choices)
            {
                if (GUILayout.Button(choice.ToString(), GUILayout.Height(50)))
                {
                    selectedChoice = new OptionSelected<T> {option = choice};
                }
            }
            EditorGUILayout.EndVertical();

            return selectedChoice;
        }
    }
}

#endif