using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

public class CardBehavior : MonoBehaviour
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
    
    private GroupsMonster group;
    public Equipement equipement;
    public GameObject equipementModel;
    public Transform container;
    public int cardType;
    private int idMobInit = 0;
    public int groupId;

    public List<GameObject> equipementsList;
    private void OnEnable()
    {
        //SetCard();
    }

    public void SetCard(int type)
    {
        container = transform.parent;
        cardType = type;
        if (type == 0)
        {
            group = DataObject.MonsterList.GetGroupsMonsterById(1);
            nameText.text = group.name;
            costText.text = group.cost + " RP";
            effectText.text = "effet";
            InstantiateGroupsMonster(group, Vector3.zero, groupParent.transform);
        }
        else if(type == 1)
        {
            equipement = DataObject.WeaponList.GetWeaponWithId(1);
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
                monsterGameObject.GetComponent<Rigidbody>().useGravity = false;
                monsterGameObject.GetComponent<CapsuleCollider>().enabled = false;
                monsterGameObject.GetComponent<MonsterPrefab>().enabled = false;
                monsterGameObject.transform.localPosition = position;
                    
                monster.IdEntity = idMobInit;
                idMobInit++;
                nbMonsterInit++;

                MonsterPrefab monsterPrefab = monsterGameObject.GetComponent<MonsterPrefab>();
                monster.SetMonsterPrefab(monsterPrefab);
                monsterPrefab.SetMonster(monster);
                
                position = origPos + GroupsPosition.position[nbMonsterInit];

            }
        }
    }
}
