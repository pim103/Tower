using UnityEngine;
using WebSocketSharp;

namespace Networking
{
    public static class NetworkingController
    {
        public static string NickName { get; set; } = "";
        public static string AuthToken { get; set; } = "";
        public static string AuthRole { get; set; } = "";
        public static string CurrentRoomToken { get; set; } = "";
        public static int[] CurrentRoomMapsLevel { get; set; }
        public static string CurrentChatToken { get; set; } = "";
        public static string Environnement { get; set; }  = "";
        public static readonly string GameToken = "-dx+L-oGb:EDJ,kGcP(7lVTe^0?9nv";
        public static readonly string PublicKey = "3082010A0282010100DCAA44772E16E5134D7F40F707D0CE5964554584A0D1579112798A4CF88C18570731EC2555E32A481310B680477CAFE1AFF4AE8E5AA54019E382C435995CC29BAC05A9ECF38C64E6EAFE21F00EDA716B7C1BCF23DFA6C62D379DD98E8AD0C2FAA539E0EDDF64462615EF44384F7974C9264947E0A24C0E59B8F96EF763422BD4338EF88A61FE7F03841AF8C2889BE876F2B5EE914F66216FFA0AAFEB2A1D827C8C39C16171A6F63B01689296C899C7070203BA0D23AF401345968F285450C874EB9BE97CFA6A74C8A373FCD2ED327ECE441F4990F54D4C004EB4FF6AC52736C45DE5A54CD7640925AA59B4A319CCDDC1B963179F77A901DE24F965C6A635B9670203010001";
        public static bool IsConnected { get; set; } = false;
        public static int ConnectionClosed { get; set; } = -1;
        public static bool ConnectionStart { get; set; } = false;

        public static void RequestChatToken()
        {
            
        }
    }
}
