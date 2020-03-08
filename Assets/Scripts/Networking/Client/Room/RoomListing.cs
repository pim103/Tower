using UnityEngine;
using UnityEngine.UI;

namespace Networking.Client.Room
{
    public class RoomListing : MonoBehaviour
    {
        [SerializeField] 
        private Text textInfo;

        public RoomListing(Room roomInfo)
        {
            RoomInfo = roomInfo;
        }

        public Room RoomInfo { get; set; }

        public void SetRoomInfo(Room roomInfo)
        {
            textInfo.text = roomInfo.Name + " : " + roomInfo.MaxPlayers;
        }
        
        public void SelectRoom()
        {
            foreach (GameObject room in GameObject.FindGameObjectsWithTag("RoomList"))
            {
                room.gameObject.GetComponent<RawImage>().color = Color.white;
            }
            this.gameObject.GetComponent<RawImage>().color = Color.gray;
        } 
    }
}