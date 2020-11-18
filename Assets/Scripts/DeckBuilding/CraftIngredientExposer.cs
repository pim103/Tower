using Games.Global;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class CraftIngredientExposer : MonoBehaviour
    {
        [SerializeField] public Button ingredientButton;
        [SerializeField] public Image ingredientImage;
        public Ingredient ingredient;
    }
}
