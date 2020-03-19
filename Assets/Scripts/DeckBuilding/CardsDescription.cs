using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardsDescription 
{
    public class Card
    {
        public string name = "";
        public int cost = 0;
        public int damage = 0;
        public int firingSpeed = 0;
        public int targetingType = 0;
        public int otherEffect = 0;
        public string text = "";
    }
    public static Card GetCardStats(int type)
    {
        Card card = new Card();
        switch (type)
        {
            case 0:
                card.name = "Tourelle";
                card.damage = 1;
                card.cost = 3;
                card.firingSpeed = 5;
                card.targetingType = 0;
                card.otherEffect = 0;
                card.text = "Dégats : " + card.damage + "\nVitesse : rapide\nTir Ciblé\nPortée : courte";
                return card;
            case 1:
                card.name = "Canon";
                card.damage = 2;
                card.cost = 5;
                card.firingSpeed = 20;
                card.targetingType = 1;
                card.otherEffect = 0;
                card.text = "Dégats : " + card.damage + "\nVitesse : lente\nTir Explosif\nPortée : longue";
                return card;
            case 2:
                card.name = "Neige";
                card.damage = 1;
                card.cost = 4;
                card.firingSpeed = 10;
                card.targetingType = 0;
                card.otherEffect = 1;
                card.text = "Dégats : " + card.damage + "\nVitesse : moyenne\nTir Ciblé\nRalenti au contact\nPortée : moyenne";
                return card;
        }

        return null;
    }
}
