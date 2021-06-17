using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class CardInDeckButtonExposer : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI cardName;
        [SerializeField] public TextMeshProUGUI cardCopies;
        [SerializeField] public Button cardButton;
        public Card card;
    }
}
