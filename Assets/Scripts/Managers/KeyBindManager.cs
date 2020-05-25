using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the KeyBindManager script, it contains functionality that is specific to the KeyBinds
/// </summary>
public class KeyBindManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    //public Text up, left, down, right, action_1, action_2, spell_1, spell_2, spell_3;
    private GameObject currentKey;
    private Color32 normal = new Color32(255, 255, 255, 255);
    private Color32 selected = new Color32(239, 116, 36, 255);

    // Start is called before the first frame update
    void Start()
    {
        keys.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "Z")));
        keys.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keys.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "Q")));
        keys.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));

        keys.Add("Action 1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Action 1", "Mouse0")));
        keys.Add("Action 2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Action 2", "Mouse1")));

        keys.Add("Spell 1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Spell 1", "A")));
        keys.Add("Spell 2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Spell 2", "E")));
        keys.Add("Spell 3", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Spell 3", "R")));

        UIManager.MyInstance.up.text = keys["Up"].ToString();
        UIManager.MyInstance.down.text = keys["Down"].ToString();
        UIManager.MyInstance.left.text = keys["Left"].ToString();
        UIManager.MyInstance.right.text = keys["Right"].ToString();

        UIManager.MyInstance.action_1.text = keys["Action 1"].ToString();
        UIManager.MyInstance.action_2.text = keys["Action 2"].ToString();

        UIManager.MyInstance.spell_1.text = keys["Spell 1"].ToString();
        UIManager.MyInstance.spell_2.text = keys["Spell 2"].ToString();
        UIManager.MyInstance.spell_3.text = keys["Spell 3"].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keys["Up"]))
        {
            Debug.Log("J'ai appuyé sur keys['Up']");
        }
        if (Input.GetKeyDown(keys["Down"]))
        {
            Debug.Log("J'ai appuyé sur keys['Down']");
        }
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;

            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString();
                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;

                // Save user keys
                SaveKeys();
            }

            if (e.isMouse)
            {
                switch (e.button)
                {
                    case 0:
                        keys[currentKey.name] = KeyCode.Mouse0;
                        currentKey.transform.GetChild(0).GetComponent<Text>().text = KeyCode.Mouse0.ToString();
                        break;
                    case 1:
                        keys[currentKey.name] = KeyCode.Mouse1;
                        currentKey.transform.GetChild(0).GetComponent<Text>().text = KeyCode.Mouse1.ToString();
                        break;
                }

                currentKey.GetComponent<Image>().color = normal;
                currentKey = null;

                // Save user keys
                SaveKeys();
            }
        }
    }

    /// <summary>
    /// When user want to change a key
    /// </summary>
    public void ChangeKey(GameObject clicked)
    {
        if (currentKey != null)
        {
            currentKey.GetComponent<Image>().color = normal;
        }

        currentKey = clicked;
        currentKey.GetComponent<Image>().color = selected;
    }

    /// <summary>
    /// Save user keys
    /// </summary>
    public void SaveKeys()
    {
        foreach (var key in keys)
        {
            PlayerPrefs.SetString(key.Key, key.Value.ToString());
        }

        PlayerPrefs.Save();
    }
}