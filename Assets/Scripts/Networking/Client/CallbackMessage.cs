using System;
using Games.Defenses;
using UnityEngine;
using Utils;

namespace Networking.Client
{
    [Serializable]
    public class CallbackMessages
    {
        public CallbackMessage callbackMessages { get; set; }
    }
    
    [Serializable]
    public class CallbackMessage
    {
        public string message { get; set; } = null;
        public CallbackIdentity identity { get; set; } = null;
        public string room { get; set; } = null;
        public GameGrid maps { get; set; } = null;
        public int roleTimer { get; set; } = -1;
        public int defenseTimer { get; set; } = -1;
        public int attackTimer { get; set; } = -1;

    }
    
    [Serializable]
    public class CallbackIdentity
    {
        public int classes { get; set; } = -1;
        public int weapon { get; set; } = -1;
    }
    
    [Serializable]
    public class CallbackChatMessages
    {
        public CallbackChatMessage callbackChatMessage { get; set; }
    }
    
    [Serializable]
    public class CallbackChatMessage
    {
        public string sender { get; set; } = null;
        public string message { get; set; } = null;
    }
}