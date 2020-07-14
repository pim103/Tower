using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding {
public class CardInCollButtonExposer : MonoBehaviour {
  [SerializeField]
  public Text name;
  [SerializeField]
  public Text cost;
  [SerializeField]
  public Text copies;
  [SerializeField]
  public Text effect;
  [SerializeField]
  public Text family;
  public Card card;
}
}
