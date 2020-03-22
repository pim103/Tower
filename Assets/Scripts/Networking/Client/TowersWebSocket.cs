using System;
using System.Collections.Generic;
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
    public static WebSocket ws;
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
        var testArray = new Dictionary<string, string>();
        testArray.Add("Allo", "1");
        testArray.Add("Allo2", "2");
        
        var setSocket = new Dictionary<string, string>();
        setSocket.Add("tokenPlayer", "skdjnflqkdnjkflq");
        setSocket.Add("room", roomId);
        
        //TowerSender("SELF", "1","Player", "Hello", FromDictToString(testArray));
        TowerSender("SELF", "1","null", "setIdentity", FromDictToString(setSocket));
    }

    public void CloseConnection()
    {
        Debug.Log("Websocket Close!");
        ws.Close();
    }

    }
}
