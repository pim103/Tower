using System.Collections.Generic;
using Games.Defenses;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace DeckBuilding
{
    public class CardBehaviorInGame : MonoBehaviour
    {
        [SerializeField] 
        private TextMesh nameText;
    
        [SerializeField] 
        private TextMesh costText;
    
        [SerializeField] 
        private TextMesh effectText;

        [SerializeField] 
        public GameObject groupParent;

        [SerializeField] 
        public MeshRenderer ownMeshRenderer;
    
        [SerializeField] 
        public MeshRenderer rangeMeshRenderer;

        [SerializeField]
        public GameObject rangeSphere;
    
        public GroupsMonster group;
        public Weapon equipement;
        public GameObject equipementModel;
        public Transform ownCardContainer;
        private int idMobInit = 0;
        public int groupId;

        public GameObject meleeWeaponSlot;
        public GameObject rangedWeaponSlot;
        public GameObject helmetSlot;
        public GameObject chestSlot;
        public GameObject grievesSlot;
        public GameObject keySlot;

        [SerializeField] 
        public CapsuleCollider groupRangeCollider; 
    
        public GroupRangeBehavior groupRangeBehavior;

        public void SetCard(Card card)
        {
            ownCardContainer = transform.parent;

            if (card.GroupsMonster != null)
            {
                equipement = null;
                group = card.GroupsMonster;
                nameText.text = group.name;
                costText.text = group.cost + " RP";
                effectText.text = "effet";
                groupRangeCollider.enabled = true;
                groupRangeCollider.radius = group.radius*2;
                rangeSphere.SetActive(true);
                rangeSphere.transform.localScale = new Vector3(group.radius*4,group.radius*4,group.radius*4);
                InstantiateGroupsMonster(group, Vector3.zero, groupParent.transform);
            }
            else if(card.Weapon != null)
            {
                group = null;
                equipement = card.Weapon;
                nameText.text = equipement.equipmentName;
                costText.text = equipement.cost+" RP";
                effectText.text = "effet";
                rangeSphere.SetActive(false);
                equipementModel = Instantiate(equipement.model, groupParent.transform);
                groupRangeCollider.enabled = false;
            }
        }

        public void InstantiateGroupsMonster(GroupsMonster groups, Vector3 position, Transform pGroupParent)
        {
            Monster monster;
            int nbMonsterInit = 0;

            Vector3 origPos = position;

            groupId = groups.id;
            foreach (MonstersInGroup monstersInGroup in groups.monstersInGroupList)
            {
                for (int i = 0; i < monstersInGroup.nbMonster; i++)
                {
                    monster = DataObject.MonsterList.GetMonsterById(monstersInGroup.GetMonster().id);

                    if (monster.model != null)
                    {
                        GameObject monsterGameObject = Instantiate(monster.model, pGroupParent, true);
                        /*Rigidbody monsterRigidBody = monsterGameObject.GetComponent<Rigidbody>();
                        monsterRigidBody.useGravity = false;
                        monsterRigidBody.isKinematic = true;*/
                        monsterGameObject.GetComponent<CapsuleCollider>().enabled = false;
                        monsterGameObject.GetComponent<NavMeshAgent>().enabled = false;
                        MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                        monsterPrefab.enabled = false;
                        monsterGameObject.transform.localPosition = position;

                        monster.InitMonster(monsterPrefab);
                    }
                    
                    monster.IdEntity = idMobInit;
                    idMobInit++;
                    nbMonsterInit++;

                    position = origPos + GroupsPosition.position[nbMonsterInit];

                }
            }
        }
    }
}
