using System;

namespace Networking.Client.Room
{
    
    [Serializable]
    public class Rooms
    {
        public Room[] rooms;
    }
    [Serializable]
    public class Room
    {
        public int id;
        public string name;
        public int password;
        public int roomOwner;
        public int maxPlayers;
        public string mode;
        public bool isRanked;
        public bool isPublic;
        public bool isLaunched;
        public bool hasEnded;

        public Room(int id, string name, int password, int roomOwner, int maxPlayers, string mode, bool isRanked, bool isPublic, bool isLaunched, bool hasEnded)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.roomOwner = roomOwner;
            this.maxPlayers = maxPlayers;
            this.mode = mode;
            this.isRanked = isRanked;
            this.isPublic = isPublic;
            this.isLaunched = isLaunched;
            this.hasEnded = hasEnded;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int Password
        {
            get => password;
            set => password = value;
        }

        public int RoomOwner
        {
            get => roomOwner;
            set => roomOwner = value;
        }

        public int MaxPlayers
        {
            get => maxPlayers;
            set => maxPlayers = value;
        }

        public string Mode
        {
            get => mode;
            set => mode = value;
        }

        public bool IsRanked
        {
            get => isRanked;
            set => isRanked = value;
        }

        public bool IsPublic
        {
            get => isPublic;
            set => isPublic = value;
        }

        public bool IsLaunched
        {
            get => isLaunched;
            set => isLaunched = value;
        }

        public bool HasEnded
        {
            get => hasEnded;
            set => hasEnded = value;
        }
    }
}
