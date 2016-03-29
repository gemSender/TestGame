using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using messages;
using Google.ProtocolBuffers;
using TaskTest.ServerFramework;
namespace TaskTest.Game
{
    public class Room
    {
        int maxFrame = 0;
        public int Capacity
        {
            get;
            private set;
        }
        public string Id
        {
            get;
            private set;
        }
        
        List<Player> players = new List<Player>();
        private Room(int capacity)
        {
            this.Capacity = capacity;
            Id = Guid.NewGuid().ToString("N");
        }

        public static Room Create(string playerId, int capacity) {
            var room = new Room(capacity);
            //room.players.Add(new Player() { nextMsgId = 1, id = playerId });
            return room;
        }

        public void GetMessage(GameSession session, messages.GenMessage msg)
        {
            int frame = msg.Frame;
            if (maxFrame < frame)
                maxFrame = frame;
            string pId = msg.PId;
            var player = players.Find(x => x.id == pId);
            if (player.GetCommand(msg))
            {
                player.TryRemoveHead();
                player.session = session;
                int msgId = msg.MsgId;
                if (player.nextMsgId <= msgId)
                {
                    player.nextMsgId = msgId;
                }
                var msgBytes = msg.ToByteArray();
                for (int i = 0, iMax = players.Count; i < iMax; i++)
                {
                    var sess = players[i].session;
                    if (sess != null)
                    {
                        sess.Send(msgBytes, 0, msgBytes.Length);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ignore Command");
            }
        }

        public world_messages.EnterRoomResult AddPlayer(string playerId, out string[] outPlayers)
        {
            var playerItem = players.Find(x => x.id == playerId);
            if (playerItem == null)
            {
                if (players.Count >= Capacity)
                {
                    outPlayers = null;
                    return world_messages.EnterRoomResult.OutOfCapacity;
                }

                players.Add(new Player() { id = playerId, nextMsgId = 1 });
                outPlayers = new string[players.Count];
                for (int i = 0; i < players.Count; i++)
                {
                    var p = players[i];
                    outPlayers[i] = p.id;
                }
                return world_messages.EnterRoomResult.Ok;
            }
            else {
                outPlayers = null;
                return world_messages.EnterRoomResult.AlreadyIn;
            }
        }
        //public void ReadyForGame(GameSession session, string playerId)
        //{
        //    var playerItem = players.Find(x => x.id == playerId);
        //    if(playerItem != null){
        //        playerItem.session = session;
        //        FlatBufferBuilder builder = new FlatBufferBuilder(1);
        //        var vec = GenMessage.CreateGenMessage(builder, MessageType.ReadyForGame, builder.CreateString(playerId), 0, Messages.GenMessage.CreateBufVector(builder, new byte[0]), 2);
        //        builder.Finish(vec.Value);
        //        for (int i = 0, iMax = players.Count; i < iMax; i++)
        //        {
        //            var item = players[i];

        //            for (int j = 0, jMax = players.Count; j < iMax; j++) 
        //            {
        //                players[j].session.Send(builder.DataBuffer);
        //            }
        //        }
        //    }
        //}

        public GenMessage GetCommand(string playerId, int frame)
        {
            var playerItem = players.Find(x => x.id == playerId);
            if (playerItem != null)
            {
                return playerItem.commands.Find(x => x.Frame == frame);
            }
            return null;
        }
        public int PlayerCount
        {
            get
            {
                return players.Count;
            }
        }
    }
}
