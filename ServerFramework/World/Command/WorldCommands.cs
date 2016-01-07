using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase;
using TaskTest.Game;
using WorldMessages;
using TaskTest.Utility;
using FlatBuffers;
namespace TaskTest.ServerFramework
{
    public class CreateRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            var seg = msg.GetBuffBytes().Value;
            int capacity = BitConverter.ToInt32(seg.Array, seg.Offset);
            var room = World.Instance.CreateRoom(session, capacity);
            FlatBufferBuilder fb = new FlatBufferBuilder(1);
            var vec = CreateRoomReply.CreateCreateRoomReply(fb, 0, fb.CreateString(room.Id), capacity);
            fb.Finish(vec.Value);
            session.Reply(MessageType.CreateRoomReply, msg.MsgId, fb.DataBuffer.GetArraySegment());
        }
    }

    public class GetRoomList : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            var rooms = World.Instance.GetRoomList();
            int len = rooms.Count;
            FlatBuffers.FlatBufferBuilder builder = new FlatBuffers.FlatBufferBuilder(1);
            FlatBuffers.Offset<WorldMessages.Room>[] roomOffs = new FlatBuffers.Offset<WorldMessages.Room>[len];
            for (int i = 0; i < len; i++)
            {
                var item = rooms[i];
                var vec = WorldMessages.Room.CreateRoom(builder, builder.CreateString(item.Id), item.PlayerCount, item.Capacity);
                roomOffs[i] = vec;
            }
            var roomListVec = GetRoomListReply.CreateGetRoomListReply(builder, GetRoomListReply.CreateRoomVector(builder, roomOffs));
            builder.Finish(roomListVec.Value);
            session.Reply(MessageType.GetRoomListReply, msg.MsgId, builder.DataBuffer.GetArraySegment());
        }
    }

    public class EnterRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            string playerId = session.SessionID;
            var msg = requestInfo.msg;
            var bufseg = msg.GetBuffBytes().Value;
            string roomId = System.Text.Encoding.UTF8.GetString(bufseg.Array, bufseg.Offset, bufseg.Count);
            string[] players;
            var result = World.Instance.EnterRoom(session, roomId, out players);
            StringOffset[] offsets;
            FlatBufferBuilder builder = new FlatBufferBuilder(1);
            if (result == EnterRoomResult.Ok)
            {   
                offsets = new StringOffset[players.Length];
                int roomIdLen = System.Text.Encoding.UTF8.GetByteCount(roomId);
                int playerIdLen = System.Text.Encoding.UTF8.GetByteCount(playerId);
                byte[] bytes = new byte[sizeof(int) * 2 + roomIdLen + playerIdLen];
                int s1 = Utility.Utility.WriteStructToBuffer(roomIdLen, bytes, 0);
                int s2 = Utility.Utility.WriteStringToBuffer(roomId, bytes, s1);
                int s3 = Utility.Utility.WriteStructToBuffer(playerIdLen, bytes, s1 + s2);
                Utility.Utility.WriteStringToBuffer(playerId, bytes, s1 + s2 + s3);
                for (int i = 0; i < players.Length; i++)
                {
                    var pid = players[i];
                    offsets[i] = builder.CreateString(pid);
                    if (pid != playerId)
                    {
                        var pSession = (session.AppServer as WorldServer).GetSessionByID(pid);
                        pSession.Reply(MessageType.PlayerEnterRoom, -1, new ArraySegment<byte>(bytes));
                    }
                }
            }
            else
            {
                offsets = new StringOffset[0];
            }
            EnterRoomReply.CreateEnterRoomReply(builder, EnterRoomReply.CreatePlayersVector(builder, offsets), result);
            session.Reply(MessageType.EnterRoomReply, msg.MsgId, builder.DataBuffer.GetArraySegment());
        }
    }
}
