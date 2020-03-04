using System;
using UnityEngine;
using WebSocketSharp;

namespace Networking.Client
{
    public class TowersWebSocket : MonoBehaviour
    {
        // Start is called before the first frame update
        private WebSocket ws = new WebSocket ("wss://towers.heolia.eu/websocket");
        public string call = "{\"_CLASS\":\"Player\", \"_METHOD\":\"Hello\", \"_ARGS\":[{\"allo\":1, \"allo2\":2}]}";
        bool callback = false;
        void Start()
        {
            ws.Connect();
            ws.Send("Bonjour");
            DateTime before = DateTime.Now;
            ws.OnMessage += (sender, e) => {
                Debug.Log(e.Data);
                callback = true;
                Debug.Log(DateTime.Now - before);
            };
        }

        private void Update()
        {
            if(callback)
                ws.Close();
        }
    }
}
