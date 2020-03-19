using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DecksController : MonoBehaviour
{
    [SerializeField] 
    private GameObject[] deckPlaceholders;
    
    [SerializeField]
    private Text[] deckPlaceholderNames;

    public List<string> deckNames;
    private int numberOfDecks;

    public int selectedDeck;

    [SerializeField] 
    private Button createDeckButton;

    [SerializeField] 
    private Button modifyDeckButton;
    
    [SerializeField] 
    private Button deleteDeckButton;

    [SerializeField] 
    private Button returnButton;
    
    [SerializeField] 
    private Button selectButton;

    [SerializeField] 
    private Button[] deckSelectors;
    
    void Start()
    {
        createDeckButton.onClick.AddListener(delegate
        {
            /*SceneParams.newDeck = true;
            SceneParams.deckName = "Nouveau_Deck.txt";*/
            SceneManager.LoadScene("DeckListScene");
        });
        modifyDeckButton.onClick.AddListener(delegate
        {
            /*SceneParams.newDeck = false;
            SceneParams.deckName = deckNames[selectedDeck];*/
            SceneManager.LoadScene("DeckListScene");
        });
        deleteDeckButton.onClick.AddListener(delegate
        {
            DeleteDeck(selectedDeck);
        });
        returnButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("MenuScene");
        });
        
        selectButton.onClick.AddListener(delegate
        {
            /*SceneParams.deckName = deckNames[selectedDeck];
            SceneParams.selectedDeck = selectedDeck;*/
        });

        for(int i = 0; i< deckSelectors.Length; i++)
        {
            var i1 = i;
            deckSelectors[i1].onClick.AddListener(delegate
            {
                selectedDeck = i1;
            });
        }
        
        selectedDeck = 0;
        string path = Application.persistentDataPath + "/deck_1.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "0;0;0;0");
        }


        deckNames = Directory.GetFiles(Application.persistentDataPath).ToList();

        for(int i = 0; i < deckNames.Count; i++)
        {
            if (deckNames[i].Contains("level"))
            {
                deckNames.Remove(deckNames[i]);
                i--;
            }
        }

        for (int i = 0; i < deckNames.Count; i++)
        {
            deckPlaceholders[i].SetActive(true);
            deckNames[i] = deckNames[i].Split('/').Last();
            deckNames[i] = deckNames[i].Split('\\').Last();
            deckPlaceholderNames[i].text = deckNames[i].Split('.')[0];
        }

        deckPlaceholderNames[selectedDeck].color = Color.red;
    }

    private void DeleteDeck(int selected)
    {
        File.Delete(Application.persistentDataPath+"\\"+deckNames[selected]);
        SceneManager.LoadScene("ScrollDecks");
    }
}
