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

    private GroupsMonster group;
    public Weapon equipement;
    public GameObject equipementModel;
    public Transform ownCardContainer;
    public int cardType;
    private int idMobInit = 0;
    public int groupId;

    public GameObject meleeWeaponSlot;
    public GameObject rangedWeaponSlot;
    public GameObject helmetSlot;
    public GameObject chestSlot;
    public GameObject grievesSlot;

    [SerializeField]
    public CapsuleCollider groupRangeCollider;

    public GroupRangeBehavior groupRangeBehavior;


    private void OnEnable()
    {
        //SetCard();
    }

    public void SetCard(int type, int id)
    {
        ownCardContainer = transform.parent;
        cardType = type;
        if (type == 0)
        {
            group = DataObject.MonsterList.GetGroupsMonsterById(id);
            nameText.text = group.name;
            costText.text = group.cost + " RP";
            effectText.text = "effet";
            groupRangeCollider.radius = group.radius*2;
            rangeSphere.transform.localScale = new Vector3(group.radius*4,group.radius*4,group.radius*4);
            InstantiateGroupsMonster(group, Vector3.zero, groupParent.transform);
        }
        else if(type == 1)
        {
            equipement = DataObject.WeaponList.GetWeaponWithId(id);
            nameText.text = equipement.modelName;
            costText.text = equipement.cost+" RP";
            effectText.text = "effet";
            equipementModel = Instantiate(equipement.model, groupParent.transform);
        }
    }

    public void InstantiateGroupsMonster(GroupsMonster groups, Vector3 position, Transform pGroupParent)
    {
        Monster monster;
        int nbMonsterInit = 0;

        Vector3 origPos = position;

        groupId = groups.id;
        foreach (KeyValuePair<int, int> mobs in groups.monsterInGroups)
        {
            for (int i = 0; i < mobs.Value; i++)
            {
                monster = DataObject.MonsterList.GetMonsterById(mobs.Key);

                GameObject monsterGameObject = Instantiate(monster.model, pGroupParent, true);
                Rigidbody monsterRigidBody = monsterGameObject.GetComponent<Rigidbody>();
                monsterRigidBody.useGravity = false;
                monsterRigidBody.isKinematic = true;
                monsterGameObject.GetComponent<CapsuleCollider>().enabled = false;
                monsterGameObject.GetComponent<NavMeshAgent>().enabled = false;
                MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                /*monsterPrefab.leftHand.SetActive(false);
                monsterPrefab.rightHand.SetActive(false);*/
                monsterPrefab.enabled = false;
                monsterGameObject.transform.localPosition = position;

                monster.IdEntity = idMobInit;
                idMobInit++;
                nbMonsterInit++;

                //MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                monster.SetMonsterPrefab(monsterPrefab);
                monsterPrefab.SetMonster(monster);

                position = origPos + GroupsPosition.position[nbMonsterInit];

            }
        }
    }
}
}
