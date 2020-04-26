using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitDecklist : MonoBehaviour
{
    public List<int> cards;

    [SerializeField] 
    private InputField deckNameField;

    [SerializeField] 
    private ObjectPooler objectPooler;

    public List<int> cardTypesInDeck;

    public List<GameObject> cardFields;

    [SerializeField] 
    private Button validateButton;
    
    [SerializeField] 
    private Button returnButton;
    
    void Start()
    {
        cardFields = new List<GameObject>();
        cardTypesInDeck = new List<int>();
        /*if (SceneParams.newDeck == false)
        {
            try
            {
                string path = Application.persistentDataPath + "/" + SceneParams.deckName;
                string line;
                using (StreamReader sr = new StreamReader(path))
                {

                    line = sr.ReadLine();
                    if (line != null)
                    {
                        cards = line.Split(';').Select(Int32.Parse).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }

            deckNameField.text = SceneParams.deckName.Split('.')[0];

            foreach (var card in cards)
            {
                ShowCardInList(card);
            }
        }*/
        
        validateButton.onClick.AddListener(Validate);
            
        returnButton.onClick.AddListener(delegate
        {
            SceneManager.LoadScene("ScrollDecks");
        });
    }

    public void ShowCardInList(int card)
    {
        /*CardsDescription.Card cardDesc = CardsDescription.GetCardStats(card);
        if (!cardTypesInDeck.Contains(card))
        {
            cardFields.Add(objectPooler.GetPooledObject(0));
            GameObject cardField = cardFields.Last();
            CardInListBehavior cardInListBehavior = cardField.GetComponent<CardInListBehavior>();
            cardField.transform.SetParent(transform);
            Debug.Log(cardFields.Count);
            cardField.transform.localPosition = new Vector3(0, 150 - 30 * cardFields.Count, 0);
            cardInListBehavior.cardName.text = cardDesc.name;
            cardInListBehavior.type = card;
            
            cardField.SetActive(true);
            cardTypesInDeck.Add(card);
        }
        else
        {
            foreach (var field in cardFields)
            {
                CardInListBehavior cardInListBehavior = field.GetComponent<CardInListBehavior>();
                if (CardsDescription.GetCardStats(card).name == cardInListBehavior.cardName.text)
                {
                    cardInListBehavior.cardNumber.text = int.Parse(cardInListBehavior.cardNumber.text) + 1 + "";
                }
            }
        }*/
    }

    private void Validate()
    {
        /*try
        {
            string path = Application.persistentDataPath + "/"+SceneParams.deckName;
            string content = "";
            foreach (var card in cards)
            {
                content += card + ";";
            }

            content = content.Remove(content.Length - 1);
            
            File.Delete(path);
            File.WriteAllText(Application.persistentDataPath + "/"+deckNameField.text+".txt", content);
            SceneManager.LoadScene("ScrollDecks");
        }
        catch (Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }*/
    }
}
