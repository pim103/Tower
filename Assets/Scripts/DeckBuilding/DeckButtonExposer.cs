using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
public class DeckButtonExposer : MonoBehaviour
{
    [SerializeField] public Text deckName;
    [SerializeField] public RawImage deckImage;
    [SerializeField] public RawImage typeImage;
    public int deckId;
}
}
