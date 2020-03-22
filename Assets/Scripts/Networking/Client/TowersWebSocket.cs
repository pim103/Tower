using System;
using System.Collections.Generic;
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

    public class TowersWebSocket

    {
    // Start is called before the first frame update
    public string END_POINT;
    public string roomId;
    public static WebSocket ws;
    private string authNetwork;
    //private CanStartHandler testMessage = null;

    public TowersWebSocket(string url, string roomStatic = null)
    {
        END_POINT = url;
        roomId = roomStatic;
    }

    public void InitializeWebsocketEndpoint()
    {
        ws = new WebSocket(END_POINT);
    }

    public static void TowerSender(string target, string roomId, string rawKey = null, string rawData = null)
    {
        string json = "{";
        json += "\"_TARGET\":" + "\"" + target + "\",";
        json += "\"_ROOMID\":" + "\"" + roomId + "\",";
        json += "\""+ rawKey + "\":" + "\"" + rawData + "\"";
        json += "}";

        //Debug.Log(json);
        ws.Send(json);
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
        ws.Send(json.TrimEnd(',', ' ') + "}");
    }

    public string FromDictToString(Dictionary<string, string> dict)
    {
        string fromDict = "[{";
        foreach(KeyValuePair <string, string> keyValues in dict) {  
            fromDict += "\"" + keyValues.Key + "\":" + "\"" + keyValues.Value + "\"" + ", ";  
        }
        
        return fromDict.TrimEnd(',', ' ') + "}]";
    }

    public void StartConnection()
    {
        ws.Connect();
        
        var setSocket = new Dictionary<string, string>();
        setSocket.Add("tokenPlayer", "AA");
        setSocket.Add("room", roomId);
        
        TowerSender("SELF", "GENERAL","null", "setIdentity", FromDictToString(setSocket));
    }

    public void CloseConnection()
    {
        Debug.Log("Websocket Close!");
        ws.Close();
    }

    }
}
