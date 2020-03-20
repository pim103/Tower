using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDecklist : MonoBehaviour
{
    public class MobCards
    {
        public int deckId;
        public int mobId;
        public int copies;
    }

    public List<MobCards> cards;
}
