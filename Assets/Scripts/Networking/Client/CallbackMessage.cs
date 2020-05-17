using System;
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

        public CallbackMessage(string message)
        {
            this.message = message;
        }
        public string Message
        {
            get => message;
            set => message = value;
        }
    }
}