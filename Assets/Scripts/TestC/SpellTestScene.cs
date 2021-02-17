using System.Collections;
using System.Collections.Generic;
using System.IO;
using FullSerializer;
using Games;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
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
            classe.classe = new Classes();

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

//                    groups.InitSpecificEquipment(monster, equipment);

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
            string jsonSpell = File.ReadAllText(Application.dataPath + "/Data/SpellsJson/FirstTrajSpell.json");

            Spell spell = null;
            fsSerializer serializer = new fsSerializer();
            fsData data = fsJsonParser.Parse(jsonSpell);
            serializer.TryDeserialize(data, ref spell);

            player.spell1.text = spell.nameSpell;
            player.entity.spells.Add(spell);
            
//            Dictionary<Trigger, List<ActionTriggered>> actions = new Dictionary<Trigger, List<ActionTriggered>>();
//            actions.Add(Trigger.INTERVAL, new List<ActionTriggered>());
//
//            ActionTriggered firstAction = new ActionTriggered
//            {
//                damageDeal = 15,
//                startFrom = StartFrom.AllEnemiesInArea
//            };
//            actions[Trigger.INTERVAL].Add(firstAction);
//
//            SpellComponent firstSpellComponent = new SpellComponent
//            {
//                spellToInstantiate = new SpellToInstantiate
//                {
//                    geometry = Geometry.Sphere,
//                    scale = Vector3.one,
//                    incrementAmplitudeByTime = Vector3.one
//                },
//                trajectory = new Trajectory
//                {
//                    followCategory = FollowCategory.FOLLOW_TARGET,
////                    speed = 5,
////                    spellPath = SpellPathSerializer.TryToImportVertexPath()
//                },
//                actions = actions,
//                spellDuration = 5,
//                spellInterval = 1
//            };
//
//            Spell firstTestSpell = new Spell
//            {
//                cooldown = 0,
//                cost = 0,
//                activeSpellComponent = firstSpellComponent
//            };
//
//            player.spell1.text = "Test spell";
//            player.entity.spells.Add(firstTestSpell);
//            
//            /* ================================= 2ND SPELL ================================= */
//            SpellComponent secondSpellComponent = new SpellComponent
//            {
//                spellToInstantiate = new SpellToInstantiate
//                {
//                    geometry = Geometry.Sphere,
//                    scale = Vector3.one,
//                    height = 1
//                },
//                trajectory = new Trajectory
//                {
//                    speed = 10,
//                    spellPath = SpellPathSerializer.TryToImportSecondVertexPath(),
//                    endOfPathInstruction = EndOfPathInstruction.Loop,
//                    followCategory = FollowCategory.FOLLOW_TARGET
//                },
//                actions = actions,
//                spellDuration = 10,
//                spellInterval = 0.05f
//            };
//
//            Spell secondTestSpell = new Spell
//            {
//                cooldown = 0,
//                cost = 0,
//                activeSpellComponent = secondSpellComponent
//            };
//
//            player.spell2.text = "spell 2";
//            player.entity.spells.Add(secondTestSpell);
//            
//            /* ================================= 3ND SPELL ================================= */
//            Dictionary<Trigger, List<ActionTriggered>> actionsChild2 = new Dictionary<Trigger, List<ActionTriggered>>();
//            actionsChild2.Add(Trigger.ON_TRIGGER_ENTER, new List<ActionTriggered>());
//
//            ActionTriggered actionTriggeredChild2 = new ActionTriggered
//            {
//                damageDeal = 110,
//                startFrom = StartFrom.AllEnemiesInArea
//            };
//            actionsChild2[Trigger.ON_TRIGGER_ENTER].Add(actionTriggeredChild2);
//
//            SpellComponent spellcomp = new SpellComponent
//            {
//                spellToInstantiate = new SpellToInstantiate
//                {
//                    geometry = Geometry.Square,
//                    height = 1,
//                    scale = Vector3.one + Vector3.forward * 4 + Vector3.right * 4,
//                },
//                actions = actionsChild2
//            };
//            
//            Dictionary<Trigger, List<ActionTriggered>> actionsChild = new Dictionary<Trigger, List<ActionTriggered>>();
//            actionsChild.Add(Trigger.START, new List<ActionTriggered>());
//            
//            ActionTriggered actionTriggeredChild = new ActionTriggered
//            {
//                spellComponent = spellcomp,
//                startFrom = StartFrom.Caster
//            };
//            actionsChild[Trigger.START].Add(actionTriggeredChild);
//            
//            MovementSpell thirdSpellComponentChild = new MovementSpell
//            {
//                movementSpellType = MovementSpellType.TpWithTarget,
//                actions = actionsChild
//            };
//
//            actions.Clear();
//            actions.Add(Trigger.INTERVAL, new List<ActionTriggered>());
//
//            ActionTriggered actionTriggered = new ActionTriggered
//            {
//                spellComponent = thirdSpellComponentChild,
//                startFrom = StartFrom.RandomEnemyInArea
//            };
//            actions[Trigger.INTERVAL].Add(actionTriggered);
//            
//            SpellComponent thirdSpellComponent = new SpellComponent
//            {
//                spellToInstantiate = new SpellToInstantiate
//                {
//                    geometry = Geometry.Sphere,
//                    scale = Vector3.one + Vector3.forward * 10 + Vector3.right * 10,
//                    height = 1
//                },
//                actions = actions,
//                spellDuration = 5,
//                spellInterval = 1f
//            };
//
//            Spell thirdSpell = new Spell
//            {
//                cooldown = 0,
//                cost = 0,
//                activeSpellComponent = thirdSpellComponent,
//                startFrom = StartFrom.CursorTarget
//            };
//
//            player.spell3.text = "Compliqué";
//            player.entity.spells.Add(thirdSpell);
        }
    }
}
