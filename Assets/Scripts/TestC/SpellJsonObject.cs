using System.Collections.Generic;

namespace TestC
{
    public class SpellListObject
    {
        public List<SpellJsonObject> skills;
    }

    public class SpellJsonObject
    {
        public string id;
        public string name;
    }
}