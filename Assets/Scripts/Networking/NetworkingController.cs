using UnityEngine;
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
        public static readonly string GameToken = "-dx+L-oGb:EDJ,kGcP(7lVTe^0?9nv";
        public static readonly string PublicKey = "3082010A0282010100B79C7B178CEAD04C8D006E986D293E8882190ACA67786DD3AFE275133B1A11CAB1C49413898FC145250E2E69C1F7FB385BF419243922C900D4C48DE1EC1A45F80B31B565F6EF8C0CB93264207D14A7B48ED434E5C8F8600E02EE6E1AB265FCF955703C106A1A10ACFBE73EDE02C62C378201A72D60CDEBEF259D0E4C97B80B09FE6E56EF1D12997EE1D233EF49DC5BF7B0D587D14C988A8F7CFFEEBE79F23ACE4BE9539942FA5877880DC8D64191C0DE8971D78DDE139B9543F02B261AEC6F0B59609642E91D1EDD4DF1E34C95B502ECB430CE1C3CB354754A950A8669B0C074927ECC481383B5C927C9A5D8D16C2BFC27145C055B3497BE49F868FCBE0EFC630203010001";

        public static void RequestChatToken()
        {
            
        }
    }
}
