using System.Collections;
using System.Collections.Generic;
using Games;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
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

            if (Input.GetKeyDown(KeyCode.B))
            {
                LoadScene();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                escapeCanvas.SetActive(Cursor.visible);

                Cursor.visible = !Cursor.visible;
                Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                GroupsMonster groups = DataObject.MonsterList.GetGroupsMonsterById(6);
                InstantiateGroupsMonster(groups, Vector3.one, null);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                player.entity.hp = player.entity.initialHp;
            }
        }

        private static void LoadScene()
        {
            GameController.LoadMainMenu();
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
            while (!DictionaryManager.hasWeaponsLoad || 
                   !DictionaryManager.hasMonstersLoad || 
                   !DictionaryManager.hasCardsLoad ||
                   !DictionaryManager.hasClassesLoad)
            {
                yield return new WaitForSeconds(0.1f);
            }
            
            Identity classe = new Identity();
            // 1 : Warrior | 2 : Mage | 3 : Rogue | 4 : Ranger
            classe.InitIdentityData(IdentityType.Role, 4);

            Identity weapon = new Identity();
            // 4 : bow | 5 : sword
            weapon.InitIdentityData(IdentityType.CategoryWeapon, 4);

            ChooseDeckAndClass.currentRoleIdentity = classe;
            ChooseDeckAndClass.currentWeaponIdentity = weapon;

            player.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);

            SpellController.CastPassiveSpell(player.entity);
        }

        public void LoadSpell(Spell spell)
        {
            player.spell1.text = spell.nameSpell;
            player.entity.spells.Clear();
            player.entity.spells.Add(spell);
        }
    }
}
