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
        List<Player> players = new List<Player>();
        private Room() { }

        public static Room Create(GameSession session) {
            var room = new Room();
            room.players.Add(new Player() { nextMsgId = 1, session = session, id = session.SessionID});
            return room;
        }

        public void GetMessage(Messages.GenMessage msg)
        {
            var player = players.Find(x => x.id == msg.PId);
            int msgId = msg.MsgId;
            if (player.nextMsgId <= msgId)
            {
                player.nextMsgId = msgId;
            }
            for (int i = 0, iMax = players.Count; i < iMax; i++) {
                player.session.Send(msg.ByteBuffer);
            }
        }
    }
}
