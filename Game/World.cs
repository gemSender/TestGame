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
        Dictionary<string, Room> playerRoomDict = new Dictionary<string, Room>();
        static World mInstance;
        public Action onDestroy;
        Queue<Tuple<Func<object>, Action<GameSession, object>, GameSession>> actions = new Queue<Tuple<Func<object>,Action<GameSession, object>, GameSession>>();
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

        public void CreateRoom(GameSession session)
        {
            playerRoomDict[session.SessionID] = Room.Create(session);
        }

        public void ProcessCommand(GameSession session, Messages.GenMessage msg)
        { 
            Room room;
            if(playerRoomDict.TryGetValue(session.SessionID, out room))
            {
                room.GetMessage(msg);
            }
        }
    }
}
