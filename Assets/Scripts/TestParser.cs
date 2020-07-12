using System;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

[Serializable]
public class WeaponsListSerialize
{
    public List<WeaponJsonObject> weapons { get; set; }
}

public class TestParser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WeaponsLoad();
    }

    private void WeaponsLoad()
    {
        string jsonWeapons = "{\"weapons\":[{\"id\":\"1\",\"name\":\"Basic_Sword\",\"category\":\"0\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"5\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronSword\"},{\"id\":\"2\",\"name\":\"Basic_Bow\",\"category\":\"8\",\"type\":\"0\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"5\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"BasicBow\"},{\"id\":\"3\",\"name\":\"Basic_Spear\",\"category\":\"2\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronSpear\"},{\"id\":\"4\",\"name\":\"Basic_Dagger\",\"category\":\"10\",\"type\":\"1\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"IronDagger\"},{\"id\":\"5\",\"name\":\"Basic_Staff\",\"category\":\"9\",\"type\":\"0\",\"rarity\":\"0\",\"lootRate\":\"100\",\"cost\":\"1\",\"damage\":\"5\",\"attSpeed\":\"2\",\"onDamageDealt\":\"ApplyFire\",\"onDamageReceive\":\"KillHim\",\"model\":\"WoodenStaff\"}]}";

        fsSerializer serializer = new fsSerializer();
        fsData data;

        try
        {
            WeaponsListSerialize weaponList = null;
            data = fsJsonParser.Parse(jsonWeapons);
            serializer.TryDeserialize(data, ref weaponList);

            if (weaponList == null)
            {
                Debug.Log("Null");
                return;
            }

            foreach (WeaponJsonObject weapon in weaponList.weapons)
            {
                weapon.PrintAttribute();
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error");
            Debug.Log(e.Message);
            Debug.Log(e.Data);
        }
    }
}
