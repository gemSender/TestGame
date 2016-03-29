using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase;
using TaskTest.Game;
using world_messages;
using TaskTest.Utility;
using Google.ProtocolBuffers;
namespace TaskTest.ServerFramework
{
    public class CreateRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            var seg = msg.Buff.ToByteArray();
            int capacity = BitConverter.ToInt32(seg, 0);
            var room = World.Instance.CreateRoom(session, capacity);
            var replyMsg = MsgCreateRoomReply.CreateBuilder()
                .SetCapacity(capacity)
                .SetId(room.Id)
                .SetErrorCode(0)
                .Build();
            var replyBytes = replyMsg.ToByteArray();
            session.Reply(MessageType.CreateRoomReply, msg.MsgId, new ArraySegment<byte>(replyBytes, 0, replyBytes.Length));
        }
    }

    public class GetRoomList : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            var rooms = World.Instance.GetRoomList();
            int len = rooms.Count;
            var replyMsgBuilder = MsgGetRoomListReply.CreateBuilder();
            for (int i = 0; i < len; i++) {
                var item = rooms[i];
                replyMsgBuilder.AddRooms(world_messages.Room.CreateBuilder().SetId(item.Id).SetPlayerCount(item.PlayerCount).SetCapacity(item.Capacity));
            }
            var replyBytes = replyMsgBuilder.Build().ToByteArray();
            session.Reply(MessageType.GetRoomListReply, msg.MsgId, new ArraySegment<byte>(replyBytes, 0, replyBytes.Length));
        }
    }

    public class EnterRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            string playerId = session.SessionID;
            var msg = requestInfo.msg;
            var bufseg = msg.Buff;
            string roomId = msg.Buff.ToStringUtf8();
            string[] players;
            var result = World.Instance.EnterRoom(session, roomId, out players);
            var replyBuilder = MsgEnterRoomReply.CreateBuilder().SetResult(result);
            if (result == EnterRoomResult.Ok)
            {
                var pushMsg = MsgPlayerEnterRoom.CreateBuilder().SetPlayerId(playerId).SetRoomId(roomId).Build().ToByteArray();
                for (int i = 0; i < players.Length; i++)
                {
                    var pid = players[i];
                    replyBuilder.AddPlayers(pid);
                    if (pid != playerId)
                    {
                        var pSession = (session.AppServer as WorldServer).GetSessionByID(pid);
                        pSession.Reply(MessageType.PlayerEnterRoom, -1, new ArraySegment<byte>(pushMsg, 0, pushMsg.Length));
                    }
                }
            }
            var bytes = replyBuilder.Build().ToByteArray();
            session.Reply(MessageType.EnterRoomReply, msg.MsgId, new ArraySegment<byte>(bytes, 0, bytes.Length));
        }
    }
}
