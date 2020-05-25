#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Spells;
using Games.Global.Spells.SpellParameter;
using UnityEngine;
namespace SpellEditor
{
    public static class ListCreatedElement
    {
        public static Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        public static Dictionary<string, SpellComponent> SpellComponents = new Dictionary<string, SpellComponent>();
        public static Dictionary<string, Spell> Spell = new Dictionary<string, Spell>();
        public static Dictionary<string, SpellWithCondition> SpellWithCondition = new Dictionary<string, SpellWithCondition>();
    }
}
#endif
