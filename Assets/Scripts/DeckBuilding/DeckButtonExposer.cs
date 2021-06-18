using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class DeckButtonExposer : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI deckName;
        [SerializeField] public RawImage deckImage;
        [SerializeField] public RawImage typeImage;
        [SerializeField] public Button deckButton;
        public int deckId;
    }
}
