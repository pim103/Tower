#if UNITY_EDITOR || UNITY_EDITOR_64

using System;
using System.Collections.Generic;
using System.Linq;
using ContentEditor.UtilsEditor;
using Games.Global;
using Games.Global.Spells;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace ContentEditor.SpellEditorComposant
{
    public enum SpellComponentDurationType
    {
        HOLDING,
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
        private static SpellComponentDurationType spellComponentDurationType;
        private static CurrentSpellComponentEditor currentSpellComponentEditor;
        private static Trigger currentTriggerSelected;

        private static List<ActionTriggered> actionsInSpellComponent = new List<ActionTriggered>();
        
        // Transformation selectors
        private static CreateOrSelectComponent<Spell> attackSpellSelector;
        private static CreateOrSelectComponent<Spell> defenseSpellSelector;
        private static Dictionary<ReplaceSpell, CreateOrSelectComponent<Spell>> replaceSpellSelector;

        // Passive Selectors
        private static CreateOrSelectComponent<SpellComponent> passiveComponentSelector;

        private static Dictionary<ActionTriggered, bool> dropdownsEffectIsOpen = new Dictionary<ActionTriggered, bool>();
        private static Dictionary<ActionTriggered, bool> dropdownsConditionIsOpen = new Dictionary<ActionTriggered, bool>();
        private static Dictionary<ActionTriggered, CreateOrSelectComponent<SpellComponent>> componentChooseOrSelects;

        public static void InitSpellComponentEditor()
        {
            componentChooseOrSelects = new Dictionary<ActionTriggered, CreateOrSelectComponent<SpellComponent>>();
            actionsInSpellComponent = new List<ActionTriggered>();
            replaceSpellSelector = new Dictionary<ReplaceSpell, CreateOrSelectComponent<Spell>>();
            passiveComponentSelector = null;
            attackSpellSelector = null;
            defenseSpellSelector = null;
        }

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

            if (spellComponentEdited.TypeSpellComponent != TypeSpellComponent.Passive)
            {
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (spellComponentEdited.TypeSpellComponent != TypeSpellComponent.Summon && GUILayout.Button("Paramètre holding spell", GUILayout.Height(20)))
                {
                    spellComponentDurationType = SpellComponentDurationType.HOLDING;
                }

                if (spellComponentEdited.TypeSpellComponent == TypeSpellComponent.Summon || GUILayout.Button("Paramètre de la durée", GUILayout.Height(20)))
                {
                    spellComponentDurationType = SpellComponentDurationType.DURATION;
                }

                if (spellComponentEdited.TypeSpellComponent != TypeSpellComponent.Summon && GUILayout.Button("Paramètre des charges", GUILayout.Height(20)))
                {
                    spellComponentDurationType = SpellComponentDurationType.CHARGES;
                }
                EditorGUILayout.EndHorizontal();

                if (spellComponentDurationType == SpellComponentDurationType.HOLDING)
                {
                    spellComponentEdited.spellDuration = 0;
                    spellComponentEdited.spellCharges = 0;
                }
                else if (spellComponentDurationType == SpellComponentDurationType.DURATION)
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

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (spellComponentEdited.TypeSpellComponent == TypeSpellComponent.Classic ||
                spellComponentEdited.TypeSpellComponent == TypeSpellComponent.BasicAttack)
            {
                spellComponentEdited.damageMultiplierOnDistance = EditorGUILayout.FloatField(
                    "Multiplier de dégat en fct de la distance", spellComponentEdited.damageMultiplierOnDistance, GUILayout.Width(300));
                spellComponentEdited.appliesPlayerOnHitEffect = EditorGUILayout.Toggle(
                    "Applique extra effet du joueur au toucher", spellComponentEdited.appliesPlayerOnHitEffect, GUILayout.Width(300));

                if (spellComponentEdited.TypeSpellComponent != TypeSpellComponent.BasicAttack)
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
            EditorGUILayout.LabelField("Spécific à " + spellComponentEdited.TypeSpellComponent);

            DisplaySpecificComponentOptions(spellComponentEdited);

            EditorGUILayout.EndVertical();

            GUI.color = Color.blue;
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Editer les actions"))
            {
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

        private static GameObject summonSpellObject;
        private static Dictionary<Spell, CreateOrSelectComponent<Spell>> summonChooseOrSelect = new Dictionary<Spell, CreateOrSelectComponent<Spell>>();
        private static CreateOrSelectComponent<Spell> summonBasicAttack;
        
        private static void DisplaySpecificComponentOptions(SpellComponent spellComponentEdited)
        {
            switch (spellComponentEdited.TypeSpellComponent)
            {
                case TypeSpellComponent.Movement:
                    MovementSpell currentSpell = spellComponentEdited as MovementSpell;
                    currentSpell.isFollowingMouse = EditorGUILayout.Toggle("Is follow mouse", currentSpell.isFollowingMouse);
                    currentSpell.movementSpellType =
                        (MovementSpellType) EditorGUILayout.EnumPopup("Movement spell type", currentSpell.movementSpellType);

                    if (currentSpell.movementSpellType == MovementSpellType.Charge ||
                        currentSpell.movementSpellType == MovementSpellType.Dash)
                    {
                        currentSpell.direction =
                            (Direction) EditorGUILayout.EnumPopup("Direction", currentSpell.direction);
                    }
                    break;
                case TypeSpellComponent.Transformation:
                    if (!(spellComponentEdited is TransformationSpell currentTransformationSpellComponent))
                    {
                        return;
                    }
                    
                    attackSpellSelector ??= new CreateOrSelectComponent<Spell>(SpellEditor.GetSpells(), currentTransformationSpellComponent.newBasicAttack, "Attack spell", null);
                    defenseSpellSelector ??= new CreateOrSelectComponent<Spell>(SpellEditor.GetSpells(), currentTransformationSpellComponent.newBasicDefense, "Defense spell", null);
                    replaceSpellSelector ??= new Dictionary<ReplaceSpell, CreateOrSelectComponent<Spell>>();

                    currentTransformationSpellComponent.newBasicAttack = attackSpellSelector.DisplayOptions();
                    currentTransformationSpellComponent.newBasicDefense = defenseSpellSelector.DisplayOptions();

                    EditorGUILayout.BeginHorizontal();
                    int idx = 0;
                    foreach (ReplaceSpell replaceSpell in currentTransformationSpellComponent.newSpells.ToList())
                    {
                        idx++;
                        EditorGUILayout.BeginVertical();
                        replaceSpell.slotSpell = EditorGUILayout.IntField("Slot spell", replaceSpell.slotSpell);

                        if (!replaceSpellSelector.ContainsKey(replaceSpell))
                        {
                            replaceSpellSelector.Add(replaceSpell, 
                                new CreateOrSelectComponent<Spell>(SpellEditor.GetSpells(), replaceSpell.newSpell, "Replace spell " + idx, null));
                        }

                        replaceSpell.newSpell = replaceSpellSelector[replaceSpell].DisplayOptions();

                        if (GUILayout.Button("Supprimer ce spell"))
                        {
                            currentTransformationSpellComponent.newSpells.Remove(replaceSpell);
                            replaceSpellSelector.Remove(replaceSpell);
                        }
                        EditorGUILayout.EndVertical();
                    }
            
                    if (GUILayout.Button("Ajouter un nouveau spell"))
                    {
                        ReplaceSpell replaceSpell = new ReplaceSpell();
                        currentTransformationSpellComponent.newSpells.Add(replaceSpell);
                        replaceSpellSelector.Add(replaceSpell, new CreateOrSelectComponent<Spell>(SpellEditor.GetSpells(), null, "Replace spell " + currentTransformationSpellComponent.newSpells.Count, null));
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    break;
                case TypeSpellComponent.Passive:
                    if (!(spellComponentEdited is PassiveSpell currentPassiveSpellComponent))
                    {
                        return;
                    }
                    
                    passiveComponentSelector ??= new CreateOrSelectComponent<SpellComponent>(
                        SpellEditor.spellComponents, 
                        currentPassiveSpellComponent.permanentSpellComponent, 
                        "Passive component", 
                        null);

                    currentPassiveSpellComponent.permanentSpellComponent = passiveComponentSelector.DisplayOptions();
                    break;
                case TypeSpellComponent.BasicAttack:
                    break;
                case TypeSpellComponent.Summon:
                    if (!(spellComponentEdited is SummonSpell currentSummonSpellComponent))
                    {
                        return;
                    }

                    if (!summonSpellObject && !String.IsNullOrEmpty(currentSummonSpellComponent.pathObjectToInstantiate))
                    {
                        summonSpellObject =
                            (GameObject) Resources.Load(currentSummonSpellComponent.pathObjectToInstantiate);
                    }

                    EditorGUI.BeginChangeCheck();
                    summonSpellObject = (GameObject) EditorGUILayout.ObjectField("Summon object", summonSpellObject, typeof(GameObject));

                    if (EditorGUI.EndChangeCheck() && summonSpellObject)
                    {
                        currentSummonSpellComponent.pathObjectToInstantiate =
                            UtilEditor.GetObjectPathInRessourceFolder(summonSpellObject);
                    }
                    
                    currentSummonSpellComponent.hp =
                        EditorGUILayout.FloatField("Hp", currentSummonSpellComponent.hp);
                    currentSummonSpellComponent.attackDamage =
                        EditorGUILayout.FloatField("Attack Damage", currentSummonSpellComponent.attackDamage);
                    currentSummonSpellComponent.attackSpeed =
                        EditorGUILayout.FloatField("Attack Speed", currentSummonSpellComponent.attackSpeed);
                    currentSummonSpellComponent.moveSpeed =
                        EditorGUILayout.FloatField("Move Speed", currentSummonSpellComponent.moveSpeed);

                    currentSummonSpellComponent.canMove =
                        EditorGUILayout.Toggle("Can Move", currentSummonSpellComponent.canMove);
                    currentSummonSpellComponent.isTargetable =
                        EditorGUILayout.Toggle("Is Targetable", currentSummonSpellComponent.isTargetable);
                    currentSummonSpellComponent.isUnique =
                        EditorGUILayout.Toggle("Is Unique", currentSummonSpellComponent.isUnique);
                    currentSummonSpellComponent.nbUseSpells =
                        EditorGUILayout.IntField("Nb use spell", currentSummonSpellComponent.nbUseSpells);
                    currentSummonSpellComponent.summonNumber =
                        EditorGUILayout.IntField("Nb summon", currentSummonSpellComponent.summonNumber);

                    summonBasicAttack ??= new CreateOrSelectComponent<Spell>(SpellEditor.GetSpells(),
                        null, "Spellcomponent for action", null);

                    currentSummonSpellComponent.basicAttack = summonBasicAttack.DisplayOptions();
                    
                    // TODO : add multiple spell
                    // foreach (Spell s in currentSummonSpellComponent.spells.ToArray())
                    // {
                    //     summonChooseOrSelect[s].DisplayOptions();
                    //     if (summonChooseOrSelect.ContainsKey(s))
                    //     {
                    //         
                    //     }
                    // }
                    
                    break;
            }
        }

        private static void DisplayOneAction(Trigger trigger, ActionTriggered actionTriggered)
        {
            Color defaultColor = GUI.color;
            GUI.color = Color.red;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;
            
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Trigger type", trigger.ToString());
            EditorGUI.EndDisabledGroup();
            
            actionTriggered.startFrom = (StartFrom) EditorGUILayout.EnumPopup("L'action cible ou part de ", actionTriggered.startFrom);
            actionTriggered.percentageToTrigger = EditorGUILayout.IntField("Pourcentage de chance ", actionTriggered.percentageToTrigger);
            actionTriggered.damageDeal = EditorGUILayout.IntField("Damage ", actionTriggered.damageDeal);

            if (!componentChooseOrSelects.ContainsKey(actionTriggered))
            {
                componentChooseOrSelects.Add(actionTriggered, 
                    new CreateOrSelectComponent<SpellComponent>(SpellEditor.spellComponents, actionTriggered.spellComponent, "Spellcomponent for action ", null));
            }

            actionTriggered.spellComponent = componentChooseOrSelects[actionTriggered].DisplayOptions();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (!dropdownsEffectIsOpen.ContainsKey(actionTriggered))
            {
                dropdownsEffectIsOpen.Add(actionTriggered, false);
            }

            string labelEffectButton = "Ajouter un effet";
            if (dropdownsEffectIsOpen[actionTriggered])
            {
                labelEffectButton = "Supprimer l'effet";
            }

            if (GUILayout.Button(labelEffectButton))
            {
                dropdownsEffectIsOpen[actionTriggered] = !dropdownsEffectIsOpen[actionTriggered];

                actionTriggered.effect = !dropdownsEffectIsOpen[actionTriggered] ? null : new Effect();
            }

            if (dropdownsEffectIsOpen[actionTriggered] && actionTriggered.effect != null)
            {
                actionTriggered.actionOnEffectType = (ActionOnEffectType) EditorGUILayout.EnumPopup("Action de l'effet", actionTriggered.actionOnEffectType);
                actionTriggered.effect.nameEffect = EditorGUILayout.TextField("Nom de l'effet", actionTriggered.effect.nameEffect);
                actionTriggered.effect.typeEffect = (TypeEffect) EditorGUILayout.EnumPopup("Type d'effet", actionTriggered.effect.typeEffect);
                actionTriggered.effect.level = EditorGUILayout.IntField("Niveau", actionTriggered.effect.level);
                actionTriggered.effect.durationInSeconds = EditorGUILayout.FloatField("Durée", actionTriggered.effect.durationInSeconds);

                if (actionTriggered.actionOnEffectType == ActionOnEffectType.BUFF_ATTACK || actionTriggered.actionOnEffectType == ActionOnEffectType.BUFF_DEFENSE)
                {
                    actionTriggered.effect.durationBuff =
                        EditorGUILayout.FloatField("Durée du buff", actionTriggered.effect.durationBuff);
                }
                
                if (actionTriggered.effect.typeEffect == TypeEffect.Expulsion)
                {
                    actionTriggered.effect.directionExpul = (DirectionExpulsion) EditorGUILayout.EnumPopup("Direction expulsion", actionTriggered.effect.directionExpul);
                    actionTriggered.effect.originExpulsion = (OriginExpulsion) EditorGUILayout.EnumPopup("Origine expulsion", actionTriggered.effect.originExpulsion);
                }
            }
            
            if (!dropdownsConditionIsOpen.ContainsKey(actionTriggered))
            {
                dropdownsConditionIsOpen.Add(actionTriggered, false);
            }

            string labelCondtionButton = "Ajouter une condition";
            if (dropdownsConditionIsOpen[actionTriggered])
            {
                labelCondtionButton = "Supprimer la condition";
            }
            
            if (GUILayout.Button(labelCondtionButton))
            {
                dropdownsConditionIsOpen[actionTriggered] = !dropdownsConditionIsOpen[actionTriggered];

                actionTriggered.conditionToTrigger = !dropdownsConditionIsOpen[actionTriggered] ? null : new ConditionToTrigger();
            }

            if (dropdownsConditionIsOpen[actionTriggered] && actionTriggered.conditionToTrigger != null)
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
                foreach (ActionTriggered actionTriggered in spellComponentEdited.actions[currentTriggerSelected].ToList())
                {
                    EditorGUILayout.BeginVertical();
                    DisplayOneAction(currentTriggerSelected, actionTriggered);

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Supprimer cette action"))
                    {
                        spellComponentEdited.actions[currentTriggerSelected].Remove(actionTriggered);
                        componentChooseOrSelects.Remove(actionTriggered);
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
                componentChooseOrSelects.Add(newAction, new CreateOrSelectComponent<SpellComponent>(SpellEditor.spellComponents, null, "Spellcomponent for action", null));
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

            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Movement)
            {
                MovementSpell movSpell = spellComponent as MovementSpell;
                if (movSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en MovementSpell");
                    return;
                }

                if (movSpell.direction == Direction.UseTrajectory && !movSpell.isFollowingMouse &&
                    (movSpell.movementSpellType == MovementSpellType.Dash || movSpell.movementSpellType == MovementSpellType.Charge))
                {
                    if (movSpell.trajectory == null)
                    {
                        Debug.LogError("Ce type de movementSpell a besoin d'une trajectoire");
                        return;
                    }
                }
            }
            
            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Passive)
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
            
            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Summon)
            {
                SummonSpell summonSpell = spellComponent as SummonSpell;
                if (summonSpell == null)
                {
                    Debug.LogError("Le spell n'a pas pu être cast en SummonSpell");
                    return;
                }
            }

            if (spellComponent.TypeSpellComponent == TypeSpellComponent.Transformation)
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