﻿using UnityEngine;
using WebSocketSharp;

namespace Networking
{
    public static class NetworkingController
    {
        public static string NickName { get; set; }
        public static string AuthToken { get; set; }
        public static string AuthRole { get; set; }
        public static string CurrentRoomToken { get; set; }
        public static string CurrentChatToken { get; set; }
        public static string Environnement { get; set; }
        public static string WebSocketEndpointRooms { get; set; }
        public static string WebSocketEndpointChat { get; set; }
        public static string HttpsEndpoint { get; set; }
        public static string PublicKey { get; set; }
        
        public static readonly string GameToken = "-dx+L-oGb:EDJ,kGcP(7lVTe^0?9nv";


        public static void SetEnvironement()
        {
            if (Environnement == "DEV")
            {
                PublicKey = "3082010A0282010100C9CDD6B1F839B62E6393476FEC55E15577D19F9DC69912716156CA2E3237CE9E4CE03DF40F4576E0B4D140E0B4CC1A0C3FEBE8B55FB65698D3298A29AE3E3B6E0F107E4CAEA47DD7DC89352DA2FA5949D31407B6194AB16F461EECB278E8BA6692BB70F1AE6AD47E6798308C6C4FA0C132B75B33317F01519A6C1FEFFD489DB3942D294E6DCFD3A40E65B910133890C2D3963E3EF6F396595061C271F6FFD5D623269C548AC9BCA7D4C02199D6E94819E3CEEBEB5D871496057D2DDB0879D2E09B244EC49483B02E273ECFC78E8000AB8627C0A857D03518702AD27805C2987B8BAB502200314AC9E25C7CFDE3F2606629A3E6B9F5D1D6D7536F138E95D075810203010001";
                HttpsEndpoint = "https://towers-websocket-server.herokuapp.com";
                WebSocketEndpointRooms = "ws://localhost:8081";
                WebSocketEndpointChat = "ws://localhost:8082";
            }
            else if (Environnement == "LOCAL")
            {
                PublicKey =
                    "3082010A0282010100DCAA44772E16E5134D7F40F707D0CE5964554584A0D1579112798A4CF88C18570731EC2555E32A481310B680477CAFE1AFF4AE8E5AA54019E382C435995CC29BAC05A9ECF38C64E6EAFE21F00EDA716B7C1BCF23DFA6C62D379DD98E8AD0C2FAA539E0EDDF64462615EF44384F7974C9264947E0A24C0E59B8F96EF763422BD4338EF88A61FE7F03841AF8C2889BE876F2B5EE914F66216FFA0AAFEB2A1D827C8C39C16171A6F63B01689296C899C7070203BA0D23AF401345968F285450C874EB9BE97CFA6A74C8A373FCD2ED327ECE441F4990F54D4C004EB4FF6AC52736C45DE5A54CD7640925AA59B4A319CCDDC1B963179F77A901DE24F965C6A635B9670203010001";
                HttpsEndpoint = "http://tower-server.loc";
                WebSocketEndpointRooms = "ws://localhost:8081";
                WebSocketEndpointChat = "ws://localhost:8082";
            }
            else if (Environnement == "PROD")
            {
                PublicKey =
                    "3082010A0282010100DCAA44772E16E5134D7F40F707D0CE5964554584A0D1579112798A4CF88C18570731EC2555E32A481310B680477CAFE1AFF4AE8E5AA54019E382C435995CC29BAC05A9ECF38C64E6EAFE21F00EDA716B7C1BCF23DFA6C62D379DD98E8AD0C2FAA539E0EDDF64462615EF44384F7974C9264947E0A24C0E59B8F96EF763422BD4338EF88A61FE7F03841AF8C2889BE876F2B5EE914F66216FFA0AAFEB2A1D827C8C39C16171A6F63B01689296C899C7070203BA0D23AF401345968F285450C874EB9BE97CFA6A74C8A373FCD2ED327ECE441F4990F54D4C004EB4FF6AC52736C45DE5A54CD7640925AA59B4A319CCDDC1B963179F77A901DE24F965C6A635B9670203010001";
                HttpsEndpoint = "https://towers.heolia.eu";
                WebSocketEndpointRooms = "wss://towers.heolia.eu/websocket";
                WebSocketEndpointChat = "wss://towers.heolia.eu/chat";
            }
        }
    }
}
