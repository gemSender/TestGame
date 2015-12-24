using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using FlatBuffers;
using TaskTest.ServerFramework;
namespace TaskTest.Game
{
    public class Room
    {
        public string Id
        {
            get;
            private set;
        }
        List<Player> players = new List<Player>();
        private Room() {
            Id = new Guid().ToString();
        }

        public static Room Create(string playerId) {
            var room = new Room();
            room.players.Add(new Player() { nextMsgId = 1, id = playerId });
            return room;
        }

        public void GetMessage(GameSession session, Messages.GenMessage msg)
        {
            var player = players.Find(x => x.id == msg.PId);
            int msgId = msg.MsgId;
            if (player.nextMsgId <= msgId)
            {
                player.nextMsgId = msgId;
            }
            for (int i = 0, iMax = players.Count; i < iMax; i++) {
                session.Send(msg.ByteBuffer);
            }
        }

        public bool AddPlayer(string playerId)
        {
            players.Add(new Player() { nextMsgId = 1, id = playerId});
            return true;
        }
    }
}
