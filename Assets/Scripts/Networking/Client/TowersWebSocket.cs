using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

namespace Networking.Client
{
    [Serializable]
    public class MessageTest
    {
        [FormerlySerializedAs("Message")] 
        public string message;

        public MessageTest(string message)
        {
            this.message = message;
        }
    }
    public class TowersWebSocket
    {
        // Start is called before the first frame update
        public string END_POINT;
        public string roomId;
        public WebSocket ws;
        private string authNetwork;
        private MessageTest testMessage = null;

        public TowersWebSocket(string url, string roomStatic = null)
        {
            END_POINT = url;
            roomId = roomStatic;
        }

        public void InitializeWebsocketEndpoint()
        {
            ws = new WebSocket(END_POINT);
        }

        public void StartConnection()
        {
            ws.Connect();
        }

        public void CloseConnection()
        {
            Debug.Log("Websocket Close!");
            ws.Close();
        }
        
    }
}
