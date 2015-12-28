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

        public void MainLoop()
        { 
        }

        public void CreateRoom(WorldSession session)
        {
            rooms.Add(playerRoomDict[session.SessionID] = Room.Create(session.SessionID));
        }

        public void EnterRoom(GameSession session, string id)
        {
            Room room = rooms.Find(x => x.Id == id);
            if (room != null) {
                room.AddPlayer(session, session.SessionID);
            }
        }

        public void ProcessCommand(GameSession session, Messages.GenMessage msg)
        { 
            Room room;
            if(playerRoomDict.TryGetValue(session.SessionID, out room))
            {
                room.GetMessage(session, msg);
            }
        }
    }
}
