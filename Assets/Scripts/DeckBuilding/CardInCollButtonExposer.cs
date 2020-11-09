using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class CardInCollButtonExposer : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI cardName;
        [SerializeField] public TextMeshProUGUI cardCost;
        [SerializeField] public TextMeshProUGUI cardCopies;
        [SerializeField] public TextMeshProUGUI cardEffect;
        [SerializeField] public TextMeshProUGUI cardFamily;
        [SerializeField] public Button cardButton;
        public Card card;
    }
}
