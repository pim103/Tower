using System;
using Games.Transitions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Networking.Client
{
    [Serializable]
    public class CallbackMessages
    {
        public CallbackMessage[] callbackMessages;
    }
    
    [Serializable]
    public class CallbackMessage
    {
        public string message;
        public CallbackIdentity identity;

        public CallbackMessage(string message)
        {
            this.message = message;
        }
        public CallbackMessage(CallbackIdentity identity)
        {
            this.identity = identity;
        }
        public string Message
        {
            get => message;
            set => message = value;
        }
        
        public CallbackIdentity Identity
        {
            get => identity;
            set => identity = value;
        }
    }
    
    [Serializable]
    public class CallbackIdentity
    {
        public int classes;
        public int weapon;
        
        public CallbackIdentity(int classes, int weapon)
        {
            this.classes = classes;
            this.weapon = weapon;
        }

        public int Classes
        {
            get => classes;
            set => classes = value;
        }

        public int Weapon
        {
            get => weapon;
            set => weapon = value;
        }
    }
}