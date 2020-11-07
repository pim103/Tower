using System.Collections.Generic;
using UnityEngine;

//Author : Attika

namespace Games.Global
{
    public class Recipe
    {
        public int ID { get; set; } = -1;

        public Texture2D Sprite { get; set; }

        public List<Ingredient> CraftRecipe;
    }
}
