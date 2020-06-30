using System;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using SpellEditor.ComponentPanel;
using SpellEditor.PanelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace SpellEditor {
public class SpellComponentPanel : MonoBehaviour {
  [SerializeField]
  private InputField nameSpellComponent;
  [SerializeField]
  private Dropdown spellComponentSelector;
  [SerializeField]
  private Button save;

  [SerializeField]
  private Dropdown typeSpell;
  [SerializeField]
  private Dropdown typeDamage;
  [SerializeField]
  private Dropdown position;
  [SerializeField]
  private Dropdown direction;

  [SerializeField]
  private Toggle isBasicAttack;
  [SerializeField]
  private Toggle positionMidEntity;
  [SerializeField]
  private Toggle castByPassive;

  [SerializeField]
  private GameObject[] panelsTypeSpell;

  private Dictionary<string, GameObject> panels;

  [SerializeField]
  private ProjectilePanel projectilePanel;

  [SerializeField]
  private PassivePanel passivePanel;

  [SerializeField]
  private BuffSpellPanel buffSpellPanel;

  [SerializeField]
  private WavePanel wavePanel;

  [SerializeField]
  private TransformationPanel transformationPanel;

  [SerializeField]
  private MovementPanel movementPanel;

  [SerializeField]
  private SummonPanel summonPanel;

  [SerializeField]
  private AreaOfEffectPanel areaOfEffectPanel;

  [SerializeField]
  public Material[] geometryShaderMaterials;
  [SerializeField]
  public GameObject[] particleObjects;
  [SerializeField]
  public GameObject[] additionalMeshs;

  [SerializeField]
  private DropdownMultiSelector shadersOnGeometryDropdown;
  [SerializeField]
  private DropdownMultiSelector particleDropdown;
  [SerializeField]
  private DropdownMultiSelector addedMeshDropdown;

  // Start is called before the first frame update
  void Start() {
    save.onClick.AddListener(SaveCurrentPanel);

    panels = new Dictionary<string, GameObject>();
    foreach (GameObject go in panelsTypeSpell) {
      panels.Add(go.name, go);
    }

    string[] enumNames = Enum.GetNames(typeof(TypeSpell));
    List<string> listNames = new List<string>(enumNames);
    typeSpell.AddOptions(listNames);

    enumNames = Enum.GetNames(typeof(DamageType));
    listNames = new List<string>(enumNames);
    typeDamage.AddOptions(listNames);

    enumNames = Enum.GetNames(typeof(OriginalPosition));
    listNames = new List<string>(enumNames);
    position.AddOptions(listNames);

    enumNames = Enum.GetNames(typeof(OriginalDirection));
    listNames = new List<string>(enumNames);
    direction.AddOptions(listNames);

    typeSpell.onValueChanged.AddListener(ChangeTypeSpell);
    ChangeTypeSpell(typeSpell.value);

    spellComponentSelector.onValueChanged.AddListener(
        EditSpellComponentAfterSelectorChoice);

    shadersOnGeometryDropdown.InitDropdownMultiSelect();
    particleDropdown.InitDropdownMultiSelect();
    addedMeshDropdown.InitDropdownMultiSelect();
  }

  public void InitSpellComponentPanel() {
    List<string> listNames = new List<string>();
    listNames.Add("Nouvel Spell Component");
    listNames.AddRange(ListCreatedElement.SpellComponents.Keys.ToList());

    spellComponentSelector.options.Clear();
    spellComponentSelector.AddOptions(listNames);
  }

  private void DeactivateAllPanel() {
    foreach (GameObject go in panelsTypeSpell) {
      go.SetActive(false);
    }
  }

  public void ChangeTypeSpell(int newIndex) {
    TypeSpell newTypeSpell = (TypeSpell) newIndex;

    DeactivateAllPanel();

    switch (newTypeSpell) {
    case TypeSpell.Buff:
      panels ["BuffPanel"]
          .SetActive(true);
      buffSpellPanel.InitBuffPanel();
      break;
    case TypeSpell.Movement:
      panels ["MovementPanel"]
          .SetActive(true);
      movementPanel.InitMovementPanel();
      break;
    case TypeSpell.Passive:
      panels ["PassivePanel"]
          .SetActive(true);
      passivePanel.InitPassivePanel();
      break;
    case TypeSpell.Projectile:
      panels ["ProjectilePanel"]
          .SetActive(true);
      projectilePanel.InitProjectilePanel();
      break;
    case TypeSpell.Summon:
      panels ["SummonPanel"]
          .SetActive(true);
      summonPanel.InitSummonPanel();
      break;
    case TypeSpell.Transformation:
      panels ["TransformationPanel"]
          .SetActive(true);
      transformationPanel.InitTransformationPanel();
      break;
    case TypeSpell.Wave:
      panels ["WavePanel"]
          .SetActive(true);
      wavePanel.InitWavePanel();
      break;
    case TypeSpell.AreaOfEffect:
      panels ["AreaOfEffectPanel"]
          .SetActive(true);
      areaOfEffectPanel.InitAreaOfEffectPanel();
      break;
    }
  }

  public void SaveCurrentPanel() {
    SpellComponent spellComponentToSave = null;

    if (nameSpellComponent.text == "") {
      return;
    }

    switch ((TypeSpell) typeSpell.value) {
    case TypeSpell.Buff:
      spellComponentToSave = buffSpellPanel.SaveCurrentPanel();
      break;
    case TypeSpell.Movement:
      spellComponentToSave = movementPanel.SaveMovement();
      break;
    case TypeSpell.Passive:
      spellComponentToSave = passivePanel.SavePassive();
      break;
    case TypeSpell.Projectile:
      spellComponentToSave = projectilePanel.SaveProjectile();
      break;
    case TypeSpell.Summon:
      spellComponentToSave = summonPanel.SaveCurrentPanel();
      break;
    case TypeSpell.Transformation:
      spellComponentToSave = transformationPanel.SaveTransformation();
      break;
    case TypeSpell.Wave:
      spellComponentToSave = wavePanel.SaveCurrentPanel();
      break;
    case TypeSpell.AreaOfEffect:
      spellComponentToSave = areaOfEffectPanel.SaveCurrentPanel();
      break;
    }

    if (spellComponentToSave == null) {
      return;
    }

    spellComponentToSave.nameSpellComponent = nameSpellComponent.text;
    spellComponentToSave.damageType = (DamageType) typeDamage.value;
    spellComponentToSave.typeSpell = (TypeSpell) typeSpell.value;
    spellComponentToSave.OriginalDirection =
        (OriginalDirection) direction.value;
    spellComponentToSave.OriginalPosition = (OriginalPosition) position.value;

    spellComponentToSave.isBasicAttack = isBasicAttack.isOn;
    spellComponentToSave.needPositionToMidToEntity = positionMidEntity.isOn;
    spellComponentToSave.castByPassive = castByPassive.isOn;
    spellComponentToSave.geometryShaders =
        shadersOnGeometryDropdown.NamesToIndex().ToArray();
    spellComponentToSave.particleEffects =
        particleDropdown.NamesToIndex().ToArray();
    spellComponentToSave.AddedMeshs =
        addedMeshDropdown.NamesToIndex().ToArray();

    if (spellComponentSelector.value != 0) {
      Debug.Log("WARNING - ERASE DATA");
      string spellComponentChoose =
          spellComponentSelector.options[spellComponentSelector.value].text;

      NavBar.ModifyExistingComponent(
          ListCreatedElement.SpellComponents[spellComponentChoose],
          spellComponentToSave);
    } else {
      if (ListCreatedElement.SpellComponents.ContainsKey(
              nameSpellComponent.text)) {
        Debug.Log(
            "!!!!!!!! TRY TO CREATE SPELLCOMPONENT WITH SAME NAME - PLEASE CHOOSE ANOTHER NAME OR SELECT SPELLCOMPONENT !!!!!!!!");
        return;
      }
      ListCreatedElement.SpellComponents.Add(nameSpellComponent.text,
                                             spellComponentToSave);
    }

    ResetCurrentSpellComponent();
  }

  private void ResetCurrentSpellComponent() {
    nameSpellComponent.text = "";
    typeDamage.value = 0;
    typeSpell.value = 0;
    position.value = 0;
    direction.value = 0;
    spellComponentSelector.value = 0;

    isBasicAttack.isOn = false;
    positionMidEntity.isOn = false;
    castByPassive.isOn = false;
  }

  private void EditSpellComponentAfterSelectorChoice(int newIndex) {
    if (newIndex == 0) {
      return;
    }

    string spellComponentChoose = spellComponentSelector.options[newIndex].text;

    if (!ListCreatedElement.SpellComponents.ContainsKey(spellComponentChoose)) {
      return;
    }

    SpellComponent spellComponentSelected =
        ListCreatedElement.SpellComponents[spellComponentChoose];

    nameSpellComponent.text = spellComponentSelected.nameSpellComponent;
    typeDamage.value = (int) spellComponentSelected.damageType;
    typeSpell.value = (int) spellComponentSelected.typeSpell;
    position.value = (int) spellComponentSelected.OriginalPosition;
    direction.value = (int) spellComponentSelected.OriginalDirection;

    isBasicAttack.isOn = spellComponentSelected.isBasicAttack;
    positionMidEntity.isOn = spellComponentSelected.needPositionToMidToEntity;
    castByPassive.isOn = spellComponentSelected.castByPassive;

    switch ((TypeSpell) typeSpell.value) {
    case TypeSpell.Buff:
      buffSpellPanel.FillCurrentPanel((BuffSpell) spellComponentSelected);
      break;
    case TypeSpell.Movement:
      movementPanel.FillCurrentPanel((MovementSpell) spellComponentSelected);
      break;
    case TypeSpell.Passive:
      passivePanel.FillCurrentPanel((PassiveSpell) spellComponentSelected);
      break;
    case TypeSpell.Projectile:
      projectilePanel.FillCurrentPanel((ProjectileSpell)
                                           spellComponentSelected);
      break;
    case TypeSpell.Summon:
      summonPanel.FillCurrentPanel((SummonSpell) spellComponentSelected);
      break;
    case TypeSpell.Transformation:
      transformationPanel.FillCurrentPanel((TransformationSpell)
                                               spellComponentSelected);
      break;
    case TypeSpell.Wave:
      wavePanel.FillCurrentPanel((WaveSpell) spellComponentSelected);
      break;
    case TypeSpell.AreaOfEffect:
      areaOfEffectPanel.FillCurrentPanel((AreaOfEffectSpell)
                                             spellComponentSelected);
      break;
    }
  }
}
}
