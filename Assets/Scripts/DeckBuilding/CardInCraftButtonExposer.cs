using Games.Global;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO : make a child of CardInCollButtonExposer

namespace DeckBuilding
{
    public class CardInCraftButtonExposer : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI cardName;
        [SerializeField] public TextMeshProUGUI cardCost;
        [SerializeField] public TextMeshProUGUI cardCopies;
        [SerializeField] public TextMeshProUGUI cardEffect;
        [SerializeField] public TextMeshProUGUI cardFamily;
        [SerializeField] public Button cardButton;
        [SerializeField] public VerticalLayoutGroup cardCraftInfos;
        [SerializeField] public Button craftButton;
        public Recipe cardRecipe;
        public Card card;
    }
}
