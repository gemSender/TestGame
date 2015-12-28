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
        int maxFrame = 0;
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
            int frame = msg.Frame;
            if (maxFrame < frame)
                maxFrame = frame;
            var player = players.Find(x => x.id == msg.PId);
            player.session = session;
            int msgId = msg.MsgId;
            if (player.nextMsgId <= msgId)
            {
                player.nextMsgId = msgId;
            }
            for (int i = 0, iMax = players.Count; i < iMax; i++) {
                players[i].session.Send(msg.ByteBuffer);
            }
        }

        public bool AddPlayer(GameSession session, string playerId)
        {
            players.Add(new Player() { nextMsgId = 1, id = playerId, session = session});
            FlatBufferBuilder builder = new FlatBufferBuilder(1);
            var vec = GenMessage.CreateGenMessage(builder, MessageType.AddPlayer, builder.CreateString(playerId), 0, Messages.GenMessage.CreateBufVector(builder, new byte[0]), maxFrame + 2);
            builder.Finish(vec.Value);
            for (int i = 0, iMax = players.Count; i < iMax; i++)
            {
                players[i].session.Send(builder.DataBuffer);
            }
            return true;
        }
    }
}
