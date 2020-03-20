using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInListBehavior : MonoBehaviour
{
    [SerializeField] 
    public Text cardName;
    [SerializeField] 
    public Text cardNumber;

    public int type;
    
    [SerializeField] 
    private Button button;

    private DeckListExposer deckListExposer;
    private InitDecklist initDecklist;
    private void Start()
    {
        button.onClick.AddListener(RemoveCardFromList);
        deckListExposer = GameObject.Find("Exposer").GetComponent<DeckListExposer>();
        initDecklist = deckListExposer.initDecklist;
    }

    public void RemoveCardFromList()
    {
        initDecklist.cards.Remove(type);
        int currentNumber = int.Parse(cardNumber.text);
        if (currentNumber > 1)
        {
            cardNumber.text = (currentNumber - 1).ToString();
        }
        else
        {
            initDecklist.cardTypesInDeck.Remove(type);
            initDecklist.cardFields.Remove(gameObject);
            gameObject.SetActive(false);
        }
    }
}
