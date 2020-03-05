using System;
using UnityEngine;
using WebSocketSharp;

namespace Networking.Client
{
    public class TowersWebSocket
    {
        // Start is called before the first frame update
        public string END_POINT;
        private WebSocket ws;
        private string authNetwork;

        public TowersWebSocket(string url)
        {
            END_POINT = url;
        }
        
        public void InitializeWebsocketEndpoint()
        {
            ws = new WebSocket(END_POINT);
        }

        public void StartConnection()
        {
            ws.Connect();
        }
    }
}
