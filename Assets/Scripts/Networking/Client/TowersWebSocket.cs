using System;
using System.Collections.Generic;
using Menus;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WebSocketSharp;

namespace Networking.Client
{
    public static class TowersWebSocket

    {
        public static WebSocket wsGame;
        public static WebSocket wsChat;

        static TowersWebSocket()
        {

        }

        public static void InitializeWebsocketEndpoint()
        {
            wsGame = new WebSocket(NetworkingController.WebSocketEndpointRooms);
            wsChat = new WebSocket(NetworkingController.WebSocketEndpointChat);
        }

        public static void TowerSender(string target, string roomId, string rawKey = null, string rawData = null)
        {
            string json = "{";
            json += "\"_TARGET\":" + "\"" + target + "\",";
            json += "\"_ROOMID\":" + "\"" + roomId + "\",";
            json += "\""+ rawKey + "\":" + "\"" + rawData + "\"";
            json += "}";
            
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
            //Debug.Log(json);
            wsGame.Send(json.TrimEnd(',', ' ') + "}");
        }

        public static string FromDictToString(Dictionary<string, string> dict)
        {
            string fromDict = "[{";
            foreach(KeyValuePair <string, string> keyValues in dict) {  
                fromDict += "\"" + keyValues.Key + "\":" + "\"" + keyValues.Value + "\"" + ", ";  
            }
            
            return fromDict.TrimEnd(',', ' ') + "}]";
        }

        public static void StartConnection()
        {
            wsGame.Connect();
            
            var setSocket = new Dictionary<string, string>();
            setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
            setSocket.Add("room", NetworkingController.CurrentRoomToken);

            TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setIdentity", FromDictToString(setSocket));
        }

        public static void CloseConnection()
        {
            Debug.Log("Websocket Close!");
            wsGame.Close();
        }
    }
}
