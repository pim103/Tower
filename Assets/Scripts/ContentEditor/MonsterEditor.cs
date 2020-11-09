#if UNITY_EDITOR_64 || UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEditor;
using UnityEngine;
using Tools = Utils.Tools;

namespace ContentEditor {
public enum MonsterEditorCategory { MONSTER, GROUP, MONSTER_IN_GROUP }

public enum TreatmentInDatabase { ADD, DELETE, MODIFY }

public class MonsterInGroupTreatment {
  public TreatmentInDatabase treatment;
  public MonstersInGroup monstersInGroup;
}

public class MonsterEditor : IEditorInterface {
  public ContentGenerationEditor contentGenerationEditor;

  public Dictionary<int, GroupsMonster> origGroupsList =
      new Dictionary<int, GroupsMonster>();
  public Dictionary<int, Monster> origMonsterList =
      new Dictionary<int, Monster>();

  private bool isCreatingNewMonster;
  private bool isCreatingNewGroup;
  private MonsterEditorCategory monsterEditorCategory =
      MonsterEditorCategory.MONSTER;

  private Monster newMonster;
  private GroupsMonster newGroup;
  private GroupsMonster currentGroupMonsterEditing;
  private MonstersInGroup monstersInGroup;

  private List<string> monsterChoice = new List<string>();
  private List<int> monsterChoiceIds = new List<int>();

  public void DisplayHeaderContent() {
    GUILayout.FlexibleSpace();
    EditorGUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();

    if (GUILayout.Button("Monster Editor", GUILayout.Width(150),
                         GUILayout.Height(75))) {
      monsterEditorCategory = MonsterEditorCategory.MONSTER;
    }

    if (GUILayout.Button("Group Editor", GUILayout.Width(150),
                         GUILayout.Height(75))) {
      monsterEditorCategory = MonsterEditorCategory.GROUP;
    }

    GUILayout.FlexibleSpace();
    EditorGUILayout.EndHorizontal();
    GUILayout.FlexibleSpace();
  }

  public void DisplayBodyContent() {
    if (DataObject.MonsterList != null) {
      if (monsterEditorCategory == MonsterEditorCategory.MONSTER) {
        if (isCreatingNewMonster) {
          DisplayNewMonsterEditor();
        } else {
          DisplayListMonsterEditor();
        }
      } else if (monsterEditorCategory == MonsterEditorCategory.GROUP) {
        if (isCreatingNewGroup) {
          DisplayNewGroupEditor();
        } else {
          DisplayListGroupEditor();
        }
      } else if (monsterEditorCategory ==
                 MonsterEditorCategory.MONSTER_IN_GROUP) {
        DisplayMonsterInGroupEditor();
      }
    }
  }

  public void DisplayFooterContent() {
    if (monsterEditorCategory == MonsterEditorCategory.MONSTER) {
      string buttonLabel = "Créer un nouveau monstre";

      if (isCreatingNewMonster) {
        if (GUILayout.Button("Reset le nouveau monstre")) {
          newMonster = new Monster();
        }

        buttonLabel = "Changer les monstres existants";
      }

      if (GUILayout.Button(buttonLabel)) {
        isCreatingNewMonster = !isCreatingNewMonster;
      }
    } else if (monsterEditorCategory == MonsterEditorCategory.GROUP) {
      string buttonLabel = "Créer un nouveau groupe";

      if (isCreatingNewGroup) {
        if (GUILayout.Button("Reset le nouveau groupe")) {
          newGroup = new GroupsMonster();
        }

        buttonLabel = "Changer les groupes existants";
      }

      if (GUILayout.Button(buttonLabel)) {
        isCreatingNewGroup = !isCreatingNewGroup;
      }
    }
  }

  private void DisplayOneMonsterEditor(Monster monster) {
    EditorGUILayout.BeginVertical();

    GUILayout.BeginHorizontal();
    GUILayout.FlexibleSpace();
    GUILayout.Button(monster.sprite, GUILayout.Width(75), GUILayout.Height(75));
    GUILayout.FlexibleSpace();
    GUILayout.EndHorizontal();

    EditorGUI.BeginDisabledGroup(true);
    EditorGUILayout.IntField("ID", monster.id);
    EditorGUI.EndDisabledGroup();

    monster.mobName = EditorGUILayout.TextField("Name", monster.mobName);
    monster.hp = EditorGUILayout.FloatField("Hp", monster.hp);
    monster.def = EditorGUILayout.IntField("Defense", monster.def);
    monster.att = EditorGUILayout.FloatField("Attack", monster.att);
    monster.attSpeed =
        EditorGUILayout.FloatField("Attack speed", monster.attSpeed);
    monster.speed = EditorGUILayout.FloatField("Speed", monster.speed);
    monster.constraint = (TypeWeapon) EditorGUILayout.EnumPopup(
        "Weapon constraint", monster.constraint);
    monster.modelName =
        EditorGUILayout.TextField("Model Name", monster.modelName);
    monster.weaponOriginalId = EditorGUILayout.IntField(
        "Original weapon id", monster.weaponOriginalId);
    EditorGUILayout.LabelField("Sprite");

    monster.sprite = (Texture2D) EditorGUILayout.ObjectField(
        monster.sprite, typeof(Texture2D), false);

    EditorGUILayout.EndVertical();
  }

  private void DisplayNewMonsterEditor() {
    if (newMonster == null) {
      newMonster = new Monster();
    }

    GUILayout.FlexibleSpace();

    DisplayOneMonsterEditor(newMonster);

    if (GUILayout.Button("Sauvegarder le nouveau monstre")) {
      contentGenerationEditor.RequestSaveMonster(newMonster, true);
      newMonster = null;
    }

    GUILayout.FlexibleSpace();
  }

  private void DisplayListMonsterEditor() {
    EditorGUILayout.BeginHorizontal();

    int loop = 0;

    foreach (Monster monster in DataObject.MonsterList.monsterList) {
      DisplayOneMonsterEditor(monster);

      ++loop;
      if (loop % 6 == 0) {
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
      }
    }

    EditorGUILayout.EndHorizontal();
  }

  private void DisplayOneGroupEditor(GroupsMonster group) {
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
    group.family = (Family) EditorGUILayout.EnumPopup("Family", group.family);

    EditorGUILayout.LabelField("Sprite");
    group.sprite = (Texture2D) EditorGUILayout.ObjectField(
        group.sprite, typeof(Texture2D), false);

    if (GUILayout.Button("Editer les monstres dans ce groupe")) {
      currentGroupMonsterEditing = group;
      monsterEditorCategory = MonsterEditorCategory.MONSTER_IN_GROUP;

      CreateChoiceMonsterList();
    }

    EditorGUILayout.EndVertical();
  }

  private void DisplayNewGroupEditor() {
    if (newGroup == null) {
      newGroup = new GroupsMonster();
    }

    GUILayout.FlexibleSpace();

    DisplayOneGroupEditor(newGroup);

    if (GUILayout.Button("Sauvegarder le nouveau groupe")) {
      contentGenerationEditor.RequestSaveGroupMonster(newGroup, true, null);
      newGroup = null;
    }

    GUILayout.FlexibleSpace();
  }

  private void DisplayListGroupEditor() {
    EditorGUILayout.BeginHorizontal();

    int loop = 0;

    foreach (GroupsMonster group in DataObject.MonsterList.groupsList) {
      DisplayOneGroupEditor(group);

      ++loop;
      if (loop % 6 == 0) {
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
      }
    }

    EditorGUILayout.EndHorizontal();
  }

  private void DisplayOneMonsterInGroupEditor(MonstersInGroup monstersInGroup,
                                              int idGroup) {
    EditorGUILayout.BeginVertical();

    EditorGUI.BeginDisabledGroup(true);
    EditorGUILayout.IntField("ID", currentGroupMonsterEditing.id);
    EditorGUI.EndDisabledGroup();

    EditorGUI.BeginChangeCheck();
    int selected = monsterChoiceIds.IndexOf(monstersInGroup.GetMonsterId());
    selected =
        EditorGUILayout.Popup("Monster", selected, monsterChoice.ToArray());

    if (EditorGUI.EndChangeCheck()) {
      monstersInGroup.SetMonster(DataObject.MonsterList.monsterList.First(
          monster => monster.id == monsterChoiceIds[selected]));
    }

    monstersInGroup.nbMonster = EditorGUILayout.IntField(
        "Nombre de monstre", monstersInGroup.nbMonster);
    if (GUILayout.Button("Delete monster")) {
      currentGroupMonsterEditing.monstersInGroupList.Remove(monstersInGroup);
    }

    EditorGUILayout.EndVertical();
  }

  private void DisplayMonsterInGroupEditor() {
    int loop = 0;

    GUILayout.BeginHorizontal();

    foreach (MonstersInGroup monstersInGroup in currentGroupMonsterEditing
                 .monstersInGroupList.ToList()) {
      DisplayOneMonsterInGroupEditor(monstersInGroup,
                                     currentGroupMonsterEditing.id);

      ++loop;
      if (loop % 6 == 0) {
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
      }
    }

    if (GUILayout.Button("Ajouter un monstre", GUILayout.Width(150),
                         GUILayout.Height(50))) {
      currentGroupMonsterEditing.monstersInGroupList.Add(new MonstersInGroup());
    }

    EditorGUILayout.EndHorizontal();
  }

  private void CreateChoiceMonsterList() {
    monsterChoice.Clear();
    monsterChoiceIds.Clear();

    monsterChoice.Add("");
    monsterChoiceIds.Add(-1);

    DataObject.MonsterList.monsterList.ForEach(monster => {
      monsterChoice.Add(monster.id + " : " + monster.mobName);
      monsterChoiceIds.Add(monster.id);
    });
  }

  public List<MonsterInGroupTreatment>
  GetTreatmentForMonsterInGroup(List<MonstersInGroup> newMonstersInGroup,
                                List<MonstersInGroup> originalMonsterInGroup) {
    List<MonsterInGroupTreatment> resultList =
        new List<MonsterInGroupTreatment>();

    foreach (MonstersInGroup monstersInGroup in newMonstersInGroup) {
      if (originalMonsterInGroup.Exists(monsterInG =>
                                            monsterInG.GetMonsterId() ==
                                            monstersInGroup.GetMonsterId())) {
        MonstersInGroup orig = originalMonsterInGroup.First(
            monster =>
                monster.GetMonsterId() == monstersInGroup.GetMonsterId());

        if (monstersInGroup.nbMonster != orig.nbMonster) {
          resultList.Add(new MonsterInGroupTreatment{
              treatment = TreatmentInDatabase.MODIFY,
              monstersInGroup = monstersInGroup});
        }
      } else {
        resultList.Add(
            new MonsterInGroupTreatment{treatment = TreatmentInDatabase.ADD,
                                        monstersInGroup = monstersInGroup});
      }
    }

    originalMonsterInGroup
        .FindAll(monster => !newMonstersInGroup.Exists(
                     newMonster =>
                         newMonster.GetMonsterId() == monster.GetMonsterId()))
        .ForEach(monsterToDelete => {
          resultList.Add(new MonsterInGroupTreatment{
              treatment = TreatmentInDatabase.DELETE,
              monstersInGroup = monsterToDelete});
        });

    return resultList;
  }

  public void CloneMonsterDictionary() {
    origGroupsList.Clear();
    origMonsterList.Clear();

    foreach (Monster monster in DataObject.MonsterList.monsterList) {
      origMonsterList.Add(monster.id, Tools.Clone(monster));
    }

    foreach (GroupsMonster group in DataObject.MonsterList.groupsList) {
      GroupsMonster cloneGroup = Tools.Clone(group);
      cloneGroup.monstersInGroupList = new List<MonstersInGroup>();

      foreach (MonstersInGroup monster in group.monstersInGroupList) {
        cloneGroup.monstersInGroupList.Add(Tools.Clone(monster));
      }

      origGroupsList.Add(group.id, cloneGroup);
    }
  }
}
}
#endif