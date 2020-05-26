using Networking.Client;
using UnityEngine;

namespace Networking
{
    public class WebsocketHandler : MonoBehaviour
    {
        public CallbackMessages callbackHandlers;

        private void Start()
        {
            TowersWebSocket.wsGame.OnMessage += (sender, args) =>
            {
                if (args.Data.Contains("callbackMessages"))
                {
                    callbackHandlers = JsonUtility.FromJson<CallbackMessages>(args.Data);
                    CallbackMessage lastCallback = callbackHandlers.callbackMessages[callbackHandlers.callbackMessages.Length - 1];
                    Debug.Log(args.Data);

                    if (lastCallback.Message != null)
                    {
                        Debug.Log(lastCallback.Message);
                    }
                    
                    if (lastCallback.Identity != null)
                    {
                        Debug.Log(lastCallback.Identity.Classes + " / " + lastCallback.Identity.Weapon);
                    }
                }
            };
        }
    }
}
