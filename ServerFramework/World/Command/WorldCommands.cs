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
}
