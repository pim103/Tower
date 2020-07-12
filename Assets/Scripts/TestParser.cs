using System;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

public class TestParser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GroupMonsterLoad();
        //WeaponsLoad();
    }

    private void GroupMonsterLoad()
    {
        string json = "{\"groups\":[{\"id\":\"1\",\"family\":\"5\",\"cost\":\"10\",\"radius\":\"2\",\"monsterList\":[{\"id\":\"1\",\"typeWeapon\":\"0\",\"name\":\"Archer Squelette\",\"number\":\"2\",\"hp\":\"20\",\"def\":\"2\",\"att\":\"5\",\"speed\":\"1\",\"nbWeapon\":\"1\",\"skillListId\":[{\"id\":\"1\",\"name\":\"ThrowPoison\"},{\"id\":\"2\",\"name\":\"EjectTarget\"}],\"onDamageDealt\":\"Outch\",\"onDamageReceive\":\"Aie\",\"model\":\"SkeletonMonster\",\"weaponId\":\"2\"},{\"id\":\"2\",\"typeWeapon\":\"1\",\"name\":\"Berserker Squelette\",\"number\":\"2\",\"hp\":\"30\",\"def\":\"4\",\"att\":\"2\",\"speed\":\"1\",\"nbWeapon\":\"1\",\"skillListId\":[{\"id\":\"1\",\"name\":\"ThrowPoison\"}],\"onDamageDealt\":\"Outch\",\"onDamageReceive\":\"Aie\",\"model\":\"SkeletonMonsterBerserk\",\"weaponId\":\"1\"}]}]}";

        fsSerializer serializer = new fsSerializer();
        fsData data;

        try
        {
            GroupsMonsterList mobsList = null;
            data = fsJsonParser.Parse(json);
            serializer.TryDeserialize(data, ref mobsList);

            if (mobsList == null)
            {
                return;
            }

            foreach (GroupsJsonObject groups in mobsList.groups)
            {
                groups.PrintAttribute();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.Data);
        }
    }

    private void WeaponsLoad()
    {
        string jsonWeapons = "{\"weapons\":[{\"id\":\"1\",\"name\":\"Basic_Sword\",\"category\":\"0\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"5\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronSword\"},{\"id\":\"2\",\"name\":\"Basic_Bow\",\"category\":\"8\",\"type\":\"0\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"5\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"BasicBow\"},{\"id\":\"3\",\"name\":\"Basic_Spear\",\"category\":\"2\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronSpear\"},{\"id\":\"4\",\"name\":\"Basic_Dagger\",\"category\":\"10\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronDagger\"},{\"id\":\"5\",\"name\":\"Basic_Staff\",\"category\":\"9\",\"type\":\"0\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"WoodenStaff\"}]}";

        fsSerializer serializer = new fsSerializer();
        fsData data;

        try
        {
            WeaponJsonList weaponList = null;
            data = fsJsonParser.Parse(jsonWeapons);
            serializer.TryDeserialize(data, ref weaponList);

            if (weaponList == null)
            {
                return;
            }

            foreach (WeaponJsonObject weapon in weaponList.weapons)
            {
                weapon.PrintAttribute();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log(e.Data);
        }
    }
}
