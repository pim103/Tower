using Networking.Client;
using UnityEngine;

namespace Menus
{
    public class MenuWebsocketHandler : MonoBehaviour
    {
        public CallbackMessages callbackHandlers;

        private void Start()
        {
            TowersWebSocket.wsGame.OnMessage += (sender, args) =>
            {
                if (args.Data.Contains("callbackMessages"))
                {
                    callbackHandlers = JsonUtility.FromJson<CallbackMessages>(args.Data);
                    foreach (CallbackMessage callback in callbackHandlers.callbackMessages)
                    {
                        Debug.Log(callback.Message);
                    }
                }
            };
        }
    }
}
