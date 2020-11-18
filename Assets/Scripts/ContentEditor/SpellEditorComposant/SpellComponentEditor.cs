#if UNITY_EDITOR || UNITY_EDITOR_64

using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

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
        Actions
    }

    [Serializable]
    public class SpellComponentEditor
    {
        [SerializeField]
        private static SpellComponentDurationType spellComponentDurationType;

        [SerializeField]
        private static CurrentSpellComponentEditor currentSpellComponentEditor;

        [SerializeField]
        private static Trigger currentTriggerSelected;

        [SerializeField]
        private static List<ActionTriggered> actionsInSpellComponent = new List<ActionTriggered>();

        public static void DisplaySpellComponentEditor(SpellComponent spellComponentEdited)
        {
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
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            spellComponentEdited.nameSpellComponent = EditorGUILayout.TextField("Nom", spellComponentEdited.nameSpellComponent);
            spellComponentEdited.damageType = (DamageType) EditorGUILayout.EnumPopup("Type de dégat", spellComponentEdited.damageType);

            if (spellComponentEdited.typeSpell != TypeSpell.Passive)
            {
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
                    spellComponentEdited.spellCharges = 0;
                    spellComponentEdited.spellDuration =
                        EditorGUILayout.FloatField("Durée du spell", spellComponentEdited.spellDuration, GUILayout.Width(300));
                } else if (spellComponentDurationType == SpellComponentDurationType.CHARGES)
                {
                    spellComponentEdited.spellDuration = 0;
                    spellComponentEdited.spellCharges =
                        EditorGUILayout.IntField("Nombre de charges", spellComponentEdited.spellCharges, GUILayout.Width(300));
                    spellComponentEdited.conditionReduceCharge = (ConditionReduceCharge) EditorGUILayout.EnumPopup("Condition de réduction de charges", spellComponentEdited.conditionReduceCharge);
                }

                spellComponentEdited.spellInterval =
                    EditorGUILayout.FloatField("Intervalle du spell", spellComponentEdited.spellInterval, GUILayout.Width(300));
            }

            if (spellComponentEdited.typeSpell == TypeSpell.Classic ||
                spellComponentEdited.typeSpell == TypeSpell.BasicAttack)
            {
                spellComponentEdited.damageMultiplierOnDistance = EditorGUILayout.FloatField(
                    "Multiplier de dégat en fct de la distance", spellComponentEdited.damageMultiplierOnDistance, GUILayout.Width(300));
                spellComponentEdited.appliesPlayerOnHitEffect = EditorGUILayout.Toggle(
                    "Applique extra effet du joueur au toucher", spellComponentEdited.appliesPlayerOnHitEffect, GUILayout.Width(300));

                if (spellComponentEdited.typeSpell != TypeSpell.BasicAttack)
                {
                    spellComponentEdited.canStopProjectile = EditorGUILayout.Toggle(
                        "Peut stopper les projectiles", spellComponentEdited.canStopProjectile);
                    spellComponentEdited.stopSpellComponentAtDamageReceived = EditorGUILayout.Toggle(
                        "Le sort s'arrête en recevant des dégats", spellComponentEdited.stopSpellComponentAtDamageReceived, GUILayout.Width(300));
                }
            }

            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            EditorGUILayout.LabelField("Spécific à " + spellComponentEdited.typeSpell);

            switch (spellComponentEdited.typeSpell)
            {
                case TypeSpell.Movement:
                    MovementSpell currentSpell = spellComponentEdited as MovementSpell;
                    currentSpell.isFollowingMouse = EditorGUILayout.Toggle("Is follow mouse", currentSpell.isFollowingMouse);
                    currentSpell.movementSpellType =
                        (MovementSpellType) EditorGUILayout.EnumPopup("Movement spell type", currentSpell.movementSpellType);
                    break;
                case TypeSpell.Transformation:
                    TransformationSpell currentTransformationSpellComponent = spellComponentEdited as TransformationSpell;
                    EditorGUI.BeginChangeCheck();
                    int selectedNewAttack = currentTransformationSpellComponent.newBasicAttack != null ? SpellEditor.spellStringList.IndexOf(currentTransformationSpellComponent.newBasicAttack.nameSpell) : -1;
                    selectedNewAttack = EditorGUILayout.Popup("Nouvelle attaque de base", selectedNewAttack == -1 ? 0 : selectedNewAttack, SpellEditor.spellStringList.ToArray());

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selectedNewAttack == 0)
                        {
                            currentTransformationSpellComponent.newBasicAttack = null;
                        }
                        else
                        {
                            currentTransformationSpellComponent.newBasicAttack = SpellEditor.spells[selectedNewAttack - 1];
                        }
                    }

                    EditorGUI.BeginChangeCheck();
                    int selectedNewDefense = currentTransformationSpellComponent.newBasicDefense != null ? SpellEditor.spellStringList.IndexOf(currentTransformationSpellComponent.newBasicDefense.nameSpell) : -1;
                    selectedNewDefense = EditorGUILayout.Popup("Nouvelle défense", selectedNewDefense == -1 ? 0 : selectedNewDefense, SpellEditor.spellStringList.ToArray());

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selectedNewDefense == 0)
                        {
                            currentTransformationSpellComponent.newBasicDefense = null;
                        }
                        else
                        {
                            currentTransformationSpellComponent.newBasicDefense = SpellEditor.spells[selectedNewDefense - 1];
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    foreach (ReplaceSpell replaceSpell in currentTransformationSpellComponent.newSpells.ToList())
                    {
                        EditorGUILayout.BeginVertical();
                        replaceSpell.slotSpell = EditorGUILayout.IntField("Slot spell", replaceSpell.slotSpell);
                        
                        int selectedNewSpell = replaceSpell.newSpell != null ? SpellEditor.spellStringList.IndexOf(replaceSpell.newSpell.nameSpell) : -1;
                        selectedNewSpell = EditorGUILayout.Popup("Nouveau spell", selectedNewSpell == -1 ? 0 : selectedNewSpell, SpellEditor.spellStringList.ToArray());

                        if (EditorGUI.EndChangeCheck())
                        {
                            if (selectedNewSpell == 0)
                            {
                                replaceSpell.newSpell = null;
                            }
                            else
                            {
                                replaceSpell.newSpell = SpellEditor.spells[selectedNewSpell - 1];
                            }
                        }

                        if (GUILayout.Button("Supprimer ce spell"))
                        {
                            currentTransformationSpellComponent.newSpells.Remove(replaceSpell);
                        }
                        EditorGUILayout.EndVertical();
                    }

                    if (GUILayout.Button("Ajouter un nouveau spell"))
                    {
                        currentTransformationSpellComponent.newSpells.Add(new ReplaceSpell());
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    break;
                case TypeSpell.Passive:
                    PassiveSpell currentPassiveSpellComponent = spellComponentEdited as PassiveSpell;
                    EditorGUI.BeginChangeCheck();
                    int selected = currentPassiveSpellComponent.permanentSpellComponent != null ? SpellEditor.spellComponentStringList.IndexOf(currentPassiveSpellComponent.permanentSpellComponent.nameSpellComponent) : -1;
                    selected = EditorGUILayout.Popup("Permanent spellComponent", selected == -1 ? 0 : selected, SpellEditor.spellComponentStringList.ToArray());

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (selected == 0)
                        {
                            currentPassiveSpellComponent.permanentSpellComponent = null;
                        }
                        else
                        {
                            currentPassiveSpellComponent.permanentSpellComponent = SpellEditor.spellComponents[selected - 1];
                        }
                    }
                    break;
                case TypeSpell.BasicAttack:
                    break;
                case TypeSpell.Summon:
                    break;
            }

            EditorGUILayout.EndVertical();

            GUI.color = Color.blue;
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Editer les actions"))
            {
                InitChoiceList();
                currentSpellComponentEditor = CurrentSpellComponentEditor.Actions;
            }
            if (spellComponentEdited.spellToInstantiate != null && GUILayout.Button("Supprimer l'objet à instancier"))
            {
                spellComponentEdited.spellToInstantiate = null;
            }
            if (spellComponentEdited.trajectory != null && GUILayout.Button("Supprimer la trajectoire"))
            {
                spellComponentEdited.trajectory = null;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Sauvegarder le spellComponent"))
            {
                CheckValidSpellComponent(spellComponentEdited);
            }

            EditorGUILayout.EndVertical();
            GUI.color = defaultColor;
        }

        private static void InitChoiceList()
        {
            SpellEditor.CreateSpellComponentList();
        }

        private static bool dropdownEffectIsOpen;
        private static bool dropdownConditionIsOpen;
        
        private static void DisplayOneAction(Trigger trigger, ActionTriggered actionTriggered)
        {
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
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            string labelEffectButton = "Ajouter un effet";
            if (dropdownEffectIsOpen)
            {
                labelEffectButton = "Supprimer l'effet";
            }
            
            if (GUILayout.Button(labelEffectButton))
            {
                dropdownEffectIsOpen = !dropdownEffectIsOpen;

                if (!dropdownEffectIsOpen)
                {
                    actionTriggered.effect = null;
                }
                else
                {
                    actionTriggered.effect = new Effect();
                }
            }

            if (dropdownEffectIsOpen && actionTriggered.effect != null)
            {
                actionTriggered.effect.nameEffect = EditorGUILayout.TextField("Nom de l'effet", actionTriggered.effect.nameEffect);
                actionTriggered.effect.typeEffect = (TypeEffect) EditorGUILayout.EnumPopup("Type d'effet", actionTriggered.effect.typeEffect);
                actionTriggered.effect.level = EditorGUILayout.IntField("Niveau", actionTriggered.effect.level);
                actionTriggered.effect.durationInSeconds = EditorGUILayout.FloatField("Durée", actionTriggered.effect.durationInSeconds);

                if (actionTriggered.effect.typeEffect == TypeEffect.Expulsion)
                {
                    actionTriggered.effect.directionExpul = (DirectionExpulsion) EditorGUILayout.EnumPopup("Direction expulsion", actionTriggered.effect.directionExpul);
                    actionTriggered.effect.originExpulsion = (OriginExpulsion) EditorGUILayout.EnumPopup("Origine expulsion", actionTriggered.effect.originExpulsion);
                }
            }
            
            string labelCondtionButton = "Ajouter une condition";
            if (dropdownConditionIsOpen)
            {
                labelCondtionButton = "Supprimer la condition";
            }
            
            if (GUILayout.Button(labelCondtionButton))
            {
                dropdownConditionIsOpen = !dropdownConditionIsOpen;

                if (!dropdownConditionIsOpen)
                {
                    actionTriggered.conditionToTrigger = null;
                }
                else
                {
                    actionTriggered.conditionToTrigger = new ConditionToTrigger();
                }
            }

            if (dropdownConditionIsOpen && actionTriggered.conditionToTrigger != null)
            {
                actionTriggered.conditionToTrigger.conditionName = EditorGUILayout.TextField("Nom de la condition", actionTriggered.conditionToTrigger.conditionName);
                actionTriggered.conditionToTrigger.conditionType = (ConditionType) EditorGUILayout.EnumPopup("Condition type", actionTriggered.conditionToTrigger.conditionType);

                if (actionTriggered.conditionToTrigger.conditionType == ConditionType.MinEnemiesInArea)
                {
                    actionTriggered.conditionToTrigger.valueNeeded = EditorGUILayout.IntField("Valeur nécessaire",
                        actionTriggered.conditionToTrigger.valueNeeded);
                }
                else
                {
                    actionTriggered.conditionToTrigger.typeEffectNeeded = (TypeEffect) EditorGUILayout.EnumPopup("Type d'effet requis",
                        actionTriggered.conditionToTrigger.typeEffectNeeded);
                }
            }
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
                foreach (ActionTriggered actionTriggered in spellComponentEdited.actions[currentTriggerSelected].ToList())
                {
                    EditorGUILayout.BeginVertical();
                    DisplayOneAction(currentTriggerSelected, actionTriggered);

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Supprimer cette action"))
                    {
                        spellComponentEdited.actions[currentTriggerSelected].Remove(actionTriggered);
                    }
            
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndVertical();

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
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Ajouter une action pour " + currentTriggerSelected, GUILayout.Width(300)))
            {
                if (!spellComponentEdited.actions.ContainsKey(currentTriggerSelected))
                {
                    spellComponentEdited.actions.Add(currentTriggerSelected, new List<ActionTriggered>());
                }

                ActionTriggered newAction = new ActionTriggered();
                spellComponentEdited.actions[currentTriggerSelected].Add(newAction);
                actionsInSpellComponent.Add(newAction);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Editer le spellComponent"))
            {
                currentSpellComponentEditor = CurrentSpellComponentEditor.SpellComponent;
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private static void CheckValidSpellComponent(SpellComponent spellComponent)
        {
            foreach (ActionTriggered action in actionsInSpellComponent)
            {
                if (action.effect != null)
                {
                    Effect effectToCheck = action.effect;
                    if (effectToCheck.nameEffect.IsNullOrEmpty() || effectToCheck.durationInSeconds <= 0.0001)
                    {
                        Debug.LogError("Effet non valide");
                        return;
                    }
                }
            }

            if (spellComponent.nameSpellComponent.IsNullOrEmpty())
            {
                Debug.LogError("Le spellComponent n'a pas de nom");
                return;
            }

            if (spellComponentDurationType == SpellComponentDurationType.CHARGES)
            {
                if (spellComponent.spellCharges <= 0)
                {
                    Debug.LogError("Les charges ne sont pas renseignés");
                    return;
                }

                if (spellComponent.conditionReduceCharge == ConditionReduceCharge.None)
                {
                    Debug.LogError("Les charges n'ont pas de conditions pour diminué");
                    return;
                }
            }

            if (spellComponent.canStopProjectile && spellComponent.spellToInstantiate == null)
            {
                Debug.LogError("Un spell pouvant arrêter les projectiles doit avoir un spellToInstantiate");
                return;
            }

            if (spellComponent.typeSpell == TypeSpell.Movement)
            {
                MovementSpell movSpell = spellComponent as MovementSpell;
                if (movSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en MovementSpell");
                    return;
                }

                if (movSpell.movementSpellType == MovementSpellType.Dash ||
                    (movSpell.movementSpellType == MovementSpellType.Charge && !movSpell.isFollowingMouse))
                {
                    if (movSpell.trajectory == null)
                    {
                        Debug.LogError("Ce type de movementSpell a besoin d'une trajectoire");
                        return;
                    }
                }
            }
            
            if (spellComponent.typeSpell == TypeSpell.Passive)
            {
                PassiveSpell passSpell = spellComponent as PassiveSpell;
                if (passSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en PassiveSpell");
                    return;
                }
                if (passSpell.permanentSpellComponent == null)
                {
                    Debug.LogError("Le passive n'a pas d'effet permanent");
                    return;
                }
            }
            
            if (spellComponent.typeSpell == TypeSpell.Summon)
            {
                SummonSpell summonSpell = spellComponent as SummonSpell;
                if (summonSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en SummonSpell");
                    return;
                }
            }

            if (spellComponent.typeSpell == TypeSpell.Transformation)
            {
                TransformationSpell transSpell = spellComponent as TransformationSpell;
                if (transSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en TransformationSpell");
                    return;
                }

                if (transSpell.newSpells.Count == 0 && transSpell.newBasicAttack == null &&
                    transSpell.newBasicDefense == null)
                {
                    Debug.LogError("Le spell de transformation n'a pas de nouveaux spell renseignés");
                    return;
                }
            }

            SpellEditor.SaveSpellComponent();
        }
    }
}

#endif