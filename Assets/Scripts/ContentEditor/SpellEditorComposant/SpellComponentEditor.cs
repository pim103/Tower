using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;

namespace ContentEditor.SpellEditorComposant
{
    public enum SpellComponentDurationType
    {
        DURATION,
        CHARGES
    }

    public enum CurrentSpellComponentEditor
    {
        SpellComponent,
        Actions,
        Effect,
        Condition
    }

    public class SpellComponentEditor
    {
        private static SpellComponentDurationType spellComponentDurationType;

        private static CurrentSpellComponentEditor currentSpellComponentEditor;

        private static Trigger currentTriggerSelected;

        private static List<ConditionToTrigger> conditionToTriggers = new List<ConditionToTrigger>();
        private static List<String> conditionListString = new List<string>();
        private static List<Effect> effects = new List<Effect>();
        private static List<String> effectListString = new List<string>();

        public static void DisplaySpellComponentEditor(SpellComponent spellComponentEdited)
        {
            GUILayout.FlexibleSpace();
            if (currentSpellComponentEditor == CurrentSpellComponentEditor.SpellComponent)
            {
                PrincipalSpellComponentEditor(spellComponentEdited);
            }
            else if (currentSpellComponentEditor == CurrentSpellComponentEditor.Actions)
            {
                DisplayActionsInSpellComponent(spellComponentEdited);
            }
            GUILayout.FlexibleSpace();
        }

        public static void PrincipalSpellComponentEditor(SpellComponent spellComponentEdited)
        {
            EditorGUILayout.BeginVertical();

            spellComponentEdited.nameSpellComponent = EditorGUILayout.TextField("Nom", spellComponentEdited.nameSpellComponent);
            spellComponentEdited.damageType = (DamageType) EditorGUILayout.EnumPopup("Type de dégat", spellComponentEdited.damageType);
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Paramètre de la durée", GUILayout.Height(20)))
            {
                spellComponentDurationType = SpellComponentDurationType.DURATION;
            }

            if (GUILayout.Button("Paramètre des charges", GUILayout.Height(20)))
            {
                spellComponentDurationType = SpellComponentDurationType.CHARGES;
            }
            EditorGUILayout.EndHorizontal();

            if (spellComponentDurationType == SpellComponentDurationType.DURATION)
            {
                spellComponentEdited.spellDuration =
                    EditorGUILayout.FloatField("Durée du spell", spellComponentEdited.spellDuration, GUILayout.Width(300));
            } else if (spellComponentDurationType == SpellComponentDurationType.CHARGES)
            {
                spellComponentEdited.spellCharges =
                    EditorGUILayout.IntField("Nombre de charges", spellComponentEdited.spellCharges, GUILayout.Width(300));
                spellComponentEdited.conditionReduceCharge = (ConditionReduceCharge) EditorGUILayout.EnumPopup("Condition de réduction de charges", spellComponentEdited.conditionReduceCharge);
            }

            spellComponentEdited.spellInterval =
                EditorGUILayout.FloatField("Intervalle du spell", spellComponentEdited.spellInterval, GUILayout.Width(300));

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            spellComponentEdited.damageMultiplierOnDistance = EditorGUILayout.IntField(
                "Multiplier de dégat en fct de la distance", spellComponentEdited.damageMultiplierOnDistance, GUILayout.Width(300));
            spellComponentEdited.appliesPlayerOnHitEffect = EditorGUILayout.Toggle(
                "Applique extra effet du joueur au toucher", spellComponentEdited.appliesPlayerOnHitEffect, GUILayout.Width(300));
            spellComponentEdited.canStopProjectile = EditorGUILayout.Toggle(
                "Peut stopper les projectiles", spellComponentEdited.canStopProjectile);
            spellComponentEdited.stopSpellComponentAtDamageReceived = EditorGUILayout.Toggle(
                "Le sort s'arrête en recevant des dégats", spellComponentEdited.stopSpellComponentAtDamageReceived, GUILayout.Width(300));

            if (GUILayout.Button("Editer les actions"))
            {
                spellComponentEdited.actions.Add(Trigger.START, new List<ActionTriggered>());
                spellComponentEdited.actions[Trigger.START].Add(new ActionTriggered
                {
                    damageDeal = 10,
                });
                
                InitChoiceList();
                currentSpellComponentEditor = CurrentSpellComponentEditor.Actions;
            }
            
            EditorGUILayout.EndVertical();
        }

        private static void InitChoiceList()
        {
            SpellEditor.CreateSpellComponentList();
            
            effectListString.Clear();
            effectListString.Add("Nothing");

            effects.ForEach(effect =>
            {
                effectListString.Add(effect.nameEffect);
            });
            
            conditionListString.Clear();
            conditionListString.Add("Nothing");

            conditionToTriggers.ForEach(condition =>
            {
                conditionListString.Add(condition.conditionName);
            });
        }

        private static void DisplayOneAction(Trigger trigger, ActionTriggered actionTriggered)
        {
            EditorGUILayout.BeginVertical();
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Trigger type", trigger.ToString());
            EditorGUI.EndDisabledGroup();
            
            actionTriggered.startFrom = (StartFrom) EditorGUILayout.EnumPopup("L'action cible ou part de ", actionTriggered.startFrom);
            actionTriggered.percentageToTrigger = EditorGUILayout.IntField("Pourcentage de chance ", actionTriggered.percentageToTrigger);
            actionTriggered.damageDeal = EditorGUILayout.IntField("Damage ", actionTriggered.damageDeal);

            EditorGUI.BeginChangeCheck();
            int selected = actionTriggered.spellComponent != null ? SpellEditor.spellComponentStringList.IndexOf(actionTriggered.spellComponent.nameSpellComponent) : -1;
            selected = EditorGUILayout.Popup("SpellComponent", selected == -1 ? 0 : selected, SpellEditor.spellComponentStringList.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selected == 0)
                {
                    actionTriggered.spellComponent = null;
                }
                else
                {
                    actionTriggered.spellComponent = SpellEditor.spellComponents[selected - 1];
                }
            }
            
            EditorGUI.BeginChangeCheck();
            int selectedEffect = actionTriggered.effect != null ? effectListString.IndexOf(actionTriggered.effect.nameEffect) : -1;
            selectedEffect = EditorGUILayout.Popup("Effect", selectedEffect == -1 ? 0 : selectedEffect, effectListString.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selectedEffect == 0)
                {
                    actionTriggered.effect = null;
                }
                else
                {
                    actionTriggered.effect = effects[selectedEffect - 1];
                }
            }

            if (actionTriggered.effect != null)
            {
                actionTriggered.actionOnEffectType = (ActionOnEffectType) EditorGUILayout.EnumPopup("Action de l'effet ", actionTriggered.actionOnEffectType);
            }
            
            EditorGUI.BeginChangeCheck();
            int selectedCondition = actionTriggered.conditionToTrigger != null ? conditionListString.IndexOf(actionTriggered.conditionToTrigger.conditionName) : -1;
            selectedCondition = EditorGUILayout.Popup("Condition", selectedCondition == -1 ? 0 : selectedCondition, conditionListString.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                if (selectedCondition == 0)
                {
                    actionTriggered.conditionToTrigger = null;
                }
                else
                {
                    actionTriggered.conditionToTrigger = conditionToTriggers[selectedCondition - 1];
                }
            }
            
            EditorGUILayout.EndVertical();
        }

        public static void DisplayActionsInSpellComponent(SpellComponent spellComponentEdited)
        {
            GUILayout.BeginHorizontal();

            foreach (Trigger trigger in Enum.GetValues(typeof(Trigger)))
            {
                if (GUILayout.Button(trigger.ToString(), GUILayout.Height(20)))
                {
                    currentTriggerSelected = trigger;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();

            int loop = 0;

            if (spellComponentEdited.actions.ContainsKey(currentTriggerSelected))
            {
                foreach (ActionTriggered actionTriggered in spellComponentEdited.actions[currentTriggerSelected])
                {
                    DisplayOneAction(currentTriggerSelected, actionTriggered);
                
                    ++loop;
                    if (loop % 6 == 0)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal();
                    }
                }
            }
            
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Editer le spellComponent"))
            {
                currentSpellComponentEditor = CurrentSpellComponentEditor.SpellComponent;
            }
        }
    }
}