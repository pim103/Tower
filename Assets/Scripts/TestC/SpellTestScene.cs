using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Games;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Players;
using Games.Transitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace TestC
{
    public class SpellTestScene : MonoBehaviour
    {
        [SerializeField] private PlayerPrefab player;
        [SerializeField] private Image[] extraSpellText;

        [SerializeField] private GameObject escapeCanvas;
        [SerializeField] private Button backToEditor;
        
        // Start is called before the first frame update
        void Awake()
        {
            Identity classe = new Identity();
            classe.classe = Classes.Warrior;

            Identity weapon = new Identity();
            weapon.categoryWeapon = CategoryWeapon.SHORT_SWORD;

            ChooseDeckAndClass.currentRoleIdentity = classe;
            ChooseDeckAndClass.currentWeaponIdentity = weapon;
            backToEditor.onClick.AddListener(BackToEditorAction);
            player.gameObject.SetActive(false);

            StartCoroutine(Waiting());
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            DataObject.playerInScene.Clear();
            DataObject.playerInScene.Add(GameController.PlayerIndex, player);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Keypad0))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[3]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad1))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[4]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad2))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[5]);
            }
            
            if (Input.GetKeyUp(KeyCode.Keypad3))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[6]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad4))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[7]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad5))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[8]);
            }
            
            if (Input.GetKeyUp(KeyCode.Keypad6))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[9]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad7))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[10]);
            }

            if (Input.GetKeyUp(KeyCode.Keypad8))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[11]);
            }
            
            if (Input.GetKeyUp(KeyCode.Keypad9))
            {
                SpellController.CastSpell(player.entity, player.entity.spells[12]);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escapeCanvas.SetActive(Cursor.visible);

                if (Cursor.visible)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GroupsMonster groups = DataObject.MonsterList.GetGroupsMonsterById(6);
                InstantiateGroupsMonster(groups, Vector3.one, null);
            }
        }

        public void InstantiateGroupsMonster(GroupsMonster groups, Vector3 position, List<int> equipment)
        {
            InstantiateParameters param;
            Monster monster;
            int nbMonsterInit = 0;

            Vector3 origPos = position;

            foreach (MonstersInGroup monstersInGroup in groups.monstersInGroupList)
            {
                for (int i = 0; i < monstersInGroup.nbMonster; i++)
                {
                    monster = monstersInGroup.GetMonster();
                    
                    GameObject monsterGameObject = Instantiate(monster.model);
                    monsterGameObject.transform.position = position;

                    monster.IdEntity = DataObject.nbEntityInScene;
                    DataObject.nbEntityInScene++;
                    nbMonsterInit++;

                    MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                    monster.InitMonster(monsterPrefab);

                    groups.InitSpecificEquipment(monster, equipment);

                    position = origPos + GroupsPosition.position[nbMonsterInit];

                    DataObject.monsterInScene.Add(monster);
                }
            }
        }

        public void BackToEditorAction()
        {
            SceneManager.LoadScene("SpellEditor");
        }

        private IEnumerator Waiting()
        {
            while (!DictionaryManager.hasWeaponsLoad || !DictionaryManager.hasMonstersLoad || !DictionaryManager.hasCardsLoad)
            {
                yield return new WaitForSeconds(0.1f);
            }
            
            player.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            player.entity.spells.Clear();
            player.spell1.text = "";
            player.spell2.text = "";
            player.spell3.text = "";

            CreateTestSpell();

            int countSpells = 0;
//            foreach (KeyValuePair<string, Spell> pair in ListCreatedElement.Spell)
//            {
//                Spell copySpell = Tools.Clone(pair.Value);
//                player.entity.spells.Add(copySpell);
//
//                if (countSpells == 0)
//                {
//                    player.spell1.text = copySpell.nameSpell;
//                } else if (countSpells == 1)
//                {
//                    player.spell2.text = copySpell.nameSpell;
//                } else if (countSpells == 2)
//                {
//                    player.spell3.text = copySpell.nameSpell;
//                }
//                else
//                {
//                    extraSpellText[countSpells].gameObject.SetActive(true);
//                    extraSpellText[countSpells].transform.GetChild(0).GetComponent<Text>().text = copySpell.nameSpell;
//                }
//                
//                countSpells++;
//            }

            SpellController.CastPassiveSpell(player.entity);
        }

        private void CreateTestSpell()
        {
            Dictionary<Trigger, List<ActionTriggered>> actions = new Dictionary<Trigger, List<ActionTriggered>>();
            actions.Add(Trigger.ON_TRIGGER_ENTER, new List<ActionTriggered>());

            ActionTriggered firstAction = new ActionTriggered
            {
                damageDeal = 100,
                startFrom = StartFrom.AllEnemiesInArea
            };
            actions[Trigger.ON_TRIGGER_ENTER].Add(firstAction);
            
            MovementSpell firstSpellComponent = new MovementSpell
            {
                spellToInstantiate = new SpellToInstantiate
                {
                    geometry = Geometry.Sphere,
                    scale = Vector3.one,
                    incrementAmplitudeByTime = Vector3.one,
                    passingThroughEntity = true
                },
                trajectory = new Trajectory
                {
                    speed = 5,
                    spellPath = SpellPathSerializer.TryToImportVertexPath()
                },
                actions = actions,
                spellDuration = 5,
                spellInterval = 0.05f,
                movementSpellType = MovementSpellType.Charge
            };

            Spell firstTestSpell = new Spell
            {
                cooldown = 0,
                cost = 0,
                activeSpellComponent = firstSpellComponent
            };

            player.spell1.text = "Test spell";
            player.entity.spells.Add(firstTestSpell);
        }
    }
}
