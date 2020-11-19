using System;
using System.Collections.Generic;
using UnityEngine;

//Author : Attika

namespace Games.Global
{
    public class Recipe
    {
        public int ID { get; set; } = -1;

        public Texture2D Sprite { get; set; }

        private readonly Dictionary<Ingredient, int> craftRecipe;

        public Recipe(List<IngredientJson> ingredients)
        {
            craftRecipe = new Dictionary<Ingredient, int>();
            
            foreach (IngredientJson ij in ingredients)
            {
                craftRecipe.Add(new Ingredient
                {
                    id = Int32.Parse(ij.id),
                    IngName = ij.name
                }, Int32.Parse(ij.nbRessources));
            }
        }
        
        public Recipe(Dictionary<Ingredient, int> craftRecipe)
        {
            this.craftRecipe = craftRecipe;
        }

        public Dictionary<Ingredient, int> GetIngredients()
        {
            return craftRecipe;
        }

        public bool CanCraft(Dictionary<Ingredient, int> inventory, int nb = 1)
        {
            foreach (var recipeIngredient in craftRecipe)
            {
                if ((recipeIngredient.Value * nb) > inventory[recipeIngredient.Key])
                    return false;
            }
            return true;
        }
    }
}
