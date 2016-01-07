using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TaskTest.ServerFramework;

namespace TaskTest.Game
{
    public class World
    {
        List<Room> rooms = new List<Room>();
        Dictionary<string, Room> playerRoomDict = new Dictionary<string, Room>();
        static World mInstance;
        public Action onDestroy;
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                }
                return mInstance;
            }
        }

        public List<Room> GetRoomList()
        {
            return rooms;
        }
        public void MainLoop()
        { 
        }

        public Room CreateRoom(WorldSession session, int capacity)
        {
            Room ret = Room.Create(session.SessionID, capacity);
            rooms.Add(ret);
            return ret;
        }

        public WorldMessages.EnterRoomResult EnterRoom(WorldSession session, string id, out string[] players)
        {
            Room room = rooms.Find(x => x.Id == id);
            if (room != null) {
                playerRoomDict[session.SessionID] = room;
                lock (room)
                {
                    return room.AddPlayer(session.SessionID, out players);
                }
            }
            players = null;
            return WorldMessages.EnterRoomResult.RoomNotExists;
        }

        public void ProcessCommand(GameSession session, Messages.GenMessage msg)
        { 
            Room room;
            if (playerRoomDict.TryGetValue(session.SessionID, out room))
            {
                lock (room)
                {
                    room.GetMessage(session, msg);
                }
            }
        }

        public Room GetRoomByPlayerId(string playerId)
        {
            Room ret;
            if (playerRoomDict.TryGetValue(playerId, out ret)) {
                return ret;
            }
            return null;
        }

        public Room GetRoom(string roomId)
        {
            return rooms.Find(x => x.Id == roomId);
        }
    }
}
