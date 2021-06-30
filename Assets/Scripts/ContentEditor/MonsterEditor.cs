#if UNITY_EDITOR_64 || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using ContentEditor.UtilsEditor;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Weapons;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor
{
    public enum MonsterEditorCategory
    {
        MONSTER,
        GROUP,
        MONSTER_IN_GROUP,
        EDIT_SPELL
    }

    public enum TreatmentInDatabase
    {
        ADD,
        DELETE,
        MODIFY
    }

    public class MonsterInGroupTreatment
    {
        public TreatmentInDatabase treatment;
        public MonstersInGroup monstersInGroup;
    }

    public class MonsterEditor : IEditorInterface
    {
        public ContentGenerationEditor contentGenerationEditor;
        
        public Dictionary<int, GroupsMonster> origGroupsList = new Dictionary<int, GroupsMonster>();
        public Dictionary<int, Monster> origMonsterList = new Dictionary<int, Monster>();

        private bool isCreatingNewMonster;
        private bool isCreatingNewGroup;
        private MonsterEditorCategory monsterEditorCategory = MonsterEditorCategory.MONSTER;

        private Monster newMonster;
        private Monster currentMonster;
        private GroupsMonster newGroup;
        private GroupsMonster currentGroupMonsterEditing;
        private MonstersInGroup monstersInGroup;

        private List<string> monsterChoice = new List<string>();
        private List<int> monsterChoiceIds = new List<int>();

        private List<string> weaponChoice = new List<string>();
        private List<int> weaponChoiceIds = new List<int>();
        
        private Dictionary<Spell, CreateOrSelectComponent<Spell>> spellSelectors = new Dictionary<Spell, CreateOrSelectComponent<Spell>>();
        private CreateOrSelectComponent<Spell> newSelector;

        public void DisplayHeaderContent()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Monster Editor", GUILayout.Width(150), GUILayout.Height(50)))
            {
                monsterEditorCategory = MonsterEditorCategory.MONSTER;
            }

            if (GUILayout.Button("Group Editor", GUILayout.Width(150), GUILayout.Height(50)))
            {
                monsterEditorCategory = MonsterEditorCategory.GROUP;
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        public void DisplayBodyContent()
        {
            if (DataObject.MonsterList != null)
            {
                if (monsterEditorCategory == MonsterEditorCategory.MONSTER)
                {
                    if (isCreatingNewMonster)
                    {
                        DisplayNewMonsterEditor();
                    }
                    else
                    {
                        DisplayListMonsterEditor();
                    }
                }
                else if (monsterEditorCategory == MonsterEditorCategory.GROUP)
                {
                    if (isCreatingNewGroup)
                    {
                        DisplayNewGroupEditor();
                    }
                    else
                    {
                        DisplayListGroupEditor();
                    }
                } 
                else if (monsterEditorCategory == MonsterEditorCategory.MONSTER_IN_GROUP)
                {
                    DisplayMonsterInGroupEditor();
                } else if (monsterEditorCategory == MonsterEditorCategory.EDIT_SPELL)
                {
                    DisplaySpellForMonster();
                }
            }
        }

        public void DisplayFooterContent()
        {
            switch (monsterEditorCategory)
            {
                case MonsterEditorCategory.MONSTER:
                {
                    string buttonLabel = "Créer un nouveau monstre";

                    if (isCreatingNewMonster)
                    {
                        if (GUILayout.Button("Reset le nouveau monstre"))
                        {
                            newMonster = new Monster();
                        }
                    
                        buttonLabel = "Changer les monstres existants";
                    }

                    if (GUILayout.Button(buttonLabel))
                    {
                        isCreatingNewMonster = !isCreatingNewMonster;
                    }

                    break;
                }
                case MonsterEditorCategory.GROUP:
                {
                    string buttonLabel = "Créer un nouveau groupe";

                    if (isCreatingNewGroup)
                    {
                        if (GUILayout.Button("Reset le nouveau groupe"))
                        {
                            newGroup = new GroupsMonster();
                        }

                        buttonLabel = "Changer les groupes existants";
                    }

                    if (GUILayout.Button(buttonLabel))
                    {
                        isCreatingNewGroup = !isCreatingNewGroup;
                    }

                    break;
                }
            }
        }

        private void DisplayOneMonsterEditor(Monster monster)
        {
            Color defaultColor = GUI.color;
            
            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(monster.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", monster.id);
            EditorGUI.EndDisabledGroup();

            monster.monsterType = (MonsterType) EditorGUILayout.EnumPopup("Monster type", monster.monsterType);
            monster.mobName = EditorGUILayout.TextField("Name", monster.mobName);
            monster.hp = EditorGUILayout.FloatField("Hp", monster.hp);
            monster.def = EditorGUILayout.IntField("Defense", monster.def);
            monster.att = EditorGUILayout.FloatField("Attack", monster.att);
            monster.attSpeed = EditorGUILayout.FloatField("Attack speed", monster.attSpeed);
            monster.speed = EditorGUILayout.FloatField("Speed", monster.speed);
            monster.SetConstraint((TypeWeapon) EditorGUILayout.EnumPopup("Weapon constraint", monster.GetConstraint()));

            EditorGUILayout.LabelField("Model");
            monster.model = (GameObject)EditorGUILayout.ObjectField(monster.model, typeof(GameObject), false);

            EditorGUI.BeginChangeCheck();
            int selected = weaponChoiceIds.IndexOf(monster.weaponOriginalId);
            selected = EditorGUILayout.Popup("Original weapon", selected == -1 ? 0 : selected, weaponChoice.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                monster.weaponOriginalId = weaponChoiceIds[selected];
            }

            EditorGUILayout.LabelField("Sprite");
            monster.sprite = (Texture2D)EditorGUILayout.ObjectField(monster.sprite, typeof(Texture2D), false);

            if (!isCreatingNewMonster && GUILayout.Button("Editer les spells"))
            {
                currentMonster = monster;
                monsterEditorCategory = MonsterEditorCategory.EDIT_SPELL;
                
                spellSelectors.Clear();
                newSelector = null;
            }

            if (GUILayout.Button("Instantiate monster") && UtilEditor.IsTestScene())
            {
                Debug.Log("Need to instantiate monster");
            }

            EditorGUILayout.EndVertical();
        }

        private void DisplayNewMonsterEditor()
        {
            if (newMonster == null)
            {
                newMonster = new Monster();
            }
            
            GUILayout.FlexibleSpace();
            
            DisplayOneMonsterEditor(newMonster);

            if (GUILayout.Button("Sauvegarder le nouveau monstre"))
            {
                PrepareSaveRequest.RequestSaveMonster(newMonster, true);
                newMonster = null;
            }

            GUILayout.FlexibleSpace();
        }

        private void DisplayListMonsterEditor()
        {
            EditorGUILayout.BeginHorizontal();

            int loop = 0;

            foreach (Monster monster in DataObject.MonsterList.monsterList)
            {
                DisplayOneMonsterEditor(monster);

                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneGroupEditor(GroupsMonster group)
        {
            EditorGUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(group.sprite, GUILayout.Width(75), GUILayout.Height(75));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", group.id);
            EditorGUI.EndDisabledGroup();

            group.name = EditorGUILayout.TextField("Name", group.name);
            group.cost = EditorGUILayout.IntField("Cost", group.cost);
            group.radius = EditorGUILayout.IntField("Radius", group.radius);
            
            EditorGUI.BeginChangeCheck();
            Family family = (Family) EditorGUILayout.EnumPopup("Family", (Family)group.family);

            if (EditorGUI.EndChangeCheck())
            {
                group.family = (int) family;
            }

            EditorGUILayout.LabelField("Sprite");
            group.sprite = (Texture2D)EditorGUILayout.ObjectField(group.sprite, typeof(Texture2D), false);

            if (GUILayout.Button("Editer les monstres dans ce groupe"))
            {
                currentGroupMonsterEditing = group;
                monsterEditorCategory = MonsterEditorCategory.MONSTER_IN_GROUP;

                CreateChoiceMonsterList();
            }

            if (GUILayout.Button("Instantiate group") && UtilEditor.IsTestScene())
            {
                GameGridController.InitGroups(group, 1, 1, 1.5f);
            }

            EditorGUILayout.EndVertical();
        }

        private void DisplayNewGroupEditor()
        {
            if (newGroup == null)
            {
                newGroup = new GroupsMonster();
            }

            GUILayout.FlexibleSpace();
            
            DisplayOneGroupEditor(newGroup);

            if (GUILayout.Button("Sauvegarder le nouveau groupe"))
            {
                PrepareSaveRequest.RequestSaveGroupMonster(newGroup, true, null);
                newGroup = null;
            }

            GUILayout.FlexibleSpace();
        }

        private void DisplayListGroupEditor()
        {
            EditorGUILayout.BeginHorizontal();

            int loop = 0;

            foreach (GroupsMonster group in DataObject.MonsterList.groupsList)
            {
                DisplayOneGroupEditor(group);

                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayOneMonsterInGroupEditor(MonstersInGroup monstersInGroup, int idGroup)
        {
            Color defaultColor = GUI.color;

            GUI.color = Color.blue;
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.color = defaultColor;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("ID", currentGroupMonsterEditing.id);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();
            int selected = monsterChoiceIds.IndexOf(monstersInGroup.GetMonsterId());
            selected = EditorGUILayout.Popup("Monster", selected, monsterChoice.ToArray());

            if (EditorGUI.EndChangeCheck())
            {
                monstersInGroup.SetMonster(DataObject.MonsterList.monsterList.First(monster => monster.id == monsterChoiceIds[selected]));
            }

            monstersInGroup.nbMonster = EditorGUILayout.IntField("Nombre de monstre", monstersInGroup.nbMonster);
            if (GUILayout.Button("Delete monster"))
            {
                currentGroupMonsterEditing.monstersInGroupList.Remove(monstersInGroup);
            }

            EditorGUILayout.EndVertical();
        }
        
        private void DisplayMonsterInGroupEditor()
        {
            int loop = 0;
            
            GUILayout.BeginHorizontal();

            foreach (MonstersInGroup monstersInGroup in currentGroupMonsterEditing.monstersInGroupList.ToList())
            {
                DisplayOneMonsterInGroupEditor(monstersInGroup, currentGroupMonsterEditing.id);
                
                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            if (GUILayout.Button("Ajouter un monstre", GUILayout.Width(150),GUILayout.Height(50)))
            {
                currentGroupMonsterEditing.monstersInGroupList.Add(new MonstersInGroup());
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplaySpellForMonster()
        {
            GUILayout.BeginHorizontal();
            int loop = 0;
            List<Spell> existingSpell = DataObject.SpellList.SpellInfos.ConvertAll(s => s.spell);

            foreach (Spell spell in currentMonster.spells)
            {
                if (!spellSelectors.ContainsKey(spell))
                {
                    spellSelectors.Add(spell, new CreateOrSelectComponent<Spell>(existingSpell, spell, "Spell", null));
                }
            }

            foreach (KeyValuePair<Spell, CreateOrSelectComponent<Spell>> selector in spellSelectors)
            {
                Spell currentSpell = selector.Value.DisplayOptions();

                GUILayout.BeginVertical();
                Color defaultC = GUI.color;
                GUI.color = Color.green;
                if (GUILayout.Button("Save Spell"))
                {
                    PrepareSaveRequest.SaveSpellForMonster(currentMonster, currentSpell);
                }
                GUI.color = defaultC;
                GUI.color = Color.red;
                if (GUILayout.Button("Delete Spell"))
                {
                    PrepareSaveRequest.DeleteSpellForMonster(currentMonster, currentSpell);
                }
                GUI.color = defaultC;
                GUILayout.EndVertical();

                ++loop;
                if (loop % 6 == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                }
            }

            if (newSelector != null)
            {
                Spell currentSpell = newSelector.DisplayOptions();

                GUILayout.BeginVertical();
                Color defaultC = GUI.color;
                GUI.color = Color.green;
                if (GUILayout.Button("Save Spell"))
                {
                    spellSelectors.Add(currentSpell, newSelector);
                    PrepareSaveRequest.SaveSpellForMonster(currentMonster, currentSpell);
                }
                GUI.color = defaultC;
                GUI.color = Color.red;
                if (GUILayout.Button("Delete Spell"))
                {
                    newSelector = null;
                }
                GUI.color = defaultC;
                GUILayout.EndVertical();
            }
            
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Ajouter un spell"))
            {
                newSelector = new CreateOrSelectComponent<Spell>(existingSpell, null, "Spell", null);
            }
        }

        private void CreateChoiceMonsterList()
        {
            monsterChoice.Clear();
            monsterChoiceIds.Clear();
            
            monsterChoice.Add("");
            monsterChoiceIds.Add(-1);
            
            DataObject.MonsterList.monsterList.ForEach(monster =>
            {
                monsterChoice.Add(monster.id + " : " + monster.mobName);
                monsterChoiceIds.Add(monster.id);
            });
        }

        public void CreateWeaponChoiceList()
        {
            if (DataObject.EquipmentList == null)
            {
                return;
            }

            weaponChoice.Clear();
            weaponChoiceIds.Clear();

            weaponChoice.Add("Nothing");
            weaponChoiceIds.Add(-1);

            DataObject.EquipmentList.weapons.ForEach(weapon =>
            {
                weaponChoice.Add(weapon.id + " : " + weapon.equipmentName);
                weaponChoiceIds.Add(weapon.id);
            });
        }

        public List<MonsterInGroupTreatment> GetTreatmentForMonsterInGroup(
            List<MonstersInGroup> newMonstersInGroup,
            List<MonstersInGroup> originalMonsterInGroup)
        {
            List<MonsterInGroupTreatment> resultList = new List<MonsterInGroupTreatment>();
            
            foreach (MonstersInGroup monstersInGroup in newMonstersInGroup)
            {
                if (originalMonsterInGroup.Exists(monsterInG =>
                    monsterInG.GetMonsterId() == monstersInGroup.GetMonsterId()))
                {
                    MonstersInGroup orig = originalMonsterInGroup.First(monster =>
                        monster.GetMonsterId() == monstersInGroup.GetMonsterId());

                    if (monstersInGroup.nbMonster != orig.nbMonster)
                    {
                        resultList.Add(new MonsterInGroupTreatment
                        {
                            treatment = TreatmentInDatabase.MODIFY,
                            monstersInGroup = monstersInGroup
                        });
                    }
                }
                else
                {
                    resultList.Add(new MonsterInGroupTreatment
                    {
                        treatment = TreatmentInDatabase.ADD,
                        monstersInGroup = monstersInGroup
                    });
                }
            }

            originalMonsterInGroup.FindAll(monster =>
                !newMonstersInGroup.Exists(newMonster => 
                    newMonster.GetMonsterId() == monster.GetMonsterId())
            ).ForEach(monsterToDelete =>
            {
                resultList.Add(new MonsterInGroupTreatment
                {
                    treatment = TreatmentInDatabase.DELETE,
                    monstersInGroup = monsterToDelete
                });
            });

            return resultList;
        }

        public void CloneMonsterDictionary()
        {
            origGroupsList.Clear();
            origMonsterList.Clear();
            
            foreach (Monster monster in DataObject.MonsterList.monsterList)
            {
                origMonsterList.Add(monster.id, Tools.Clone(monster));
            }
            
            foreach (GroupsMonster group in DataObject.MonsterList.groupsList)
            {
                GroupsMonster cloneGroup = Tools.Clone(group);
                cloneGroup.monstersInGroupList = new List<MonstersInGroup>();

                foreach (MonstersInGroup monster in group.monstersInGroupList)
                {
                    cloneGroup.monstersInGroupList.Add(Tools.Clone(monster));
                }

                origGroupsList.Add(group.id, cloneGroup);
            }
        }
    }
}
#endif