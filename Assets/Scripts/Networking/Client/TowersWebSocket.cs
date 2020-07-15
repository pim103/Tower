using System;
using System.Collections.Generic;
using Menus;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

namespace Networking.Client
{
    [Serializable]
    public class CanStartHandler
    {
        [FormerlySerializedAs("Message")] 
        public string message;

        public CanStartHandler(string message)
        {
            this.message = message;
        }
    }

    public static class TowersWebSocket

    {
        // Start is called before the first frame update
        public static string END_POINT_GAME;
        public static string END_POINT_CHAT;
        public static WebSocket wsGame = null;
        public static WebSocket wsChat = null;
        //private CanStartHandler testMessage = null;

        static TowersWebSocket()
        {
            if (NetworkingController.Environnement == "PROD")
            {
                END_POINT_GAME = "wss://towers.heolia.eu/websocket";
                END_POINT_CHAT = "wss://towers.heolia.eu/chat";
            }
            else if (NetworkingController.Environnement == "LOCAL")
            {
                END_POINT_GAME = "ws://localhost:8081";
                END_POINT_CHAT = "ws://localhost:8082";
            }
        }

        public static void InitializeWebsocketEndpoint()
        {
            wsGame = new WebSocket(END_POINT_GAME);
            wsChat = new WebSocket(END_POINT_CHAT);
        }

        public static void TowerSender(string target, string roomId, string rawKey = null, string rawData = null)
        {
            string json = "{";
            json += "\"_TARGET\":" + "\"" + target + "\",";
            json += "\"_ROOMID\":" + "\"" + roomId + "\",";
            json += "\""+ rawKey + "\":" + "\"" + rawData + "\"";
            json += "}";

            //Debug.Log(json);
            wsGame.Send(json);
        }
        
        public static void TowerSender(string target, string roomId, string @class = null, string method = null, string args = null)
        {
            string json = "{";
            json += "\"_TARGET\":" + "\"" + target + "\",";
            json += "\"_ROOMID\":" + "\"" + roomId + "\",";
            if (@class != null)
            {
                json += "\"_CLASS\":" + "\"" + @class + "\",";
            }
            json += "\"_METHOD\":" + "\"" + method + "\",";
            if (args != null)
            {
                json += "\"_ARGS\":" + args;
            }

            wsGame.Send(json.TrimEnd(',', ' ') + "}");
        }
        
        public static void ChatSender(string target, string roomId, string senderName, string message)
        {
            string json = "{";
            json += "\"_TARGET\":" + "\"" + target + "\",";
            json += "\"_ROOMID\":" + "\"" + roomId + "\",";
            json += "\"_SENDER\":" + "\"" + senderName + "\",";
            json += "\"_MESSAGE\":" + "\"" + message + "\"";
            

            wsChat.Send(json.TrimEnd(',', ' ') + "}");
        }

        public static string FromDictToString(Dictionary<string, string> dict)
        {
            string fromDict = "[{";
            foreach(KeyValuePair <string, string> keyValues in dict) {  
                fromDict += "\"" + keyValues.Key + "\":" + "\"" + keyValues.Value + "\"" + ", ";  
            }
            
            return fromDict.TrimEnd(',', ' ') + "}]";
        }
        public static string FromDictToString(Dictionary<string, int> dict)
        {
            string fromDict = "[{";
            foreach(KeyValuePair <string, int> keyValues in dict) {  
                fromDict += "\"" + keyValues.Key + "\":" + "\"" + keyValues.Value + "\"" + ", ";  
            }
            
            return fromDict.TrimEnd(',', ' ') + "}]";
        }

        public static void StartConnection()
        {
            if (wsGame != null && !wsGame.IsAlive)
            {
                wsGame.Connect();
                wsChat.Connect();
            }

            var setSocket = new Dictionary<string, string>();
            setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
            setSocket.Add("room", NetworkingController.CurrentRoomToken);

            TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setIdentity", FromDictToString(setSocket));
            NetworkingController.ConnectionStart = true;
        }

        public static void CloseConnection()
        {
            Debug.Log("Websocket Close!");
            wsGame.Close();
        }
    }
}
