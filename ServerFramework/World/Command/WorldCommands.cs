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
namespace TaskTest.ServerFramework
{
    public class CreateRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            World.Instance.CreateRoom(session);
        }
    }

    public class GetRoomList : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            Console.Write("GetRoomList, Id: {0}", msg.MsgId);
            var rooms = World.Instance.GetRoomList();
            int len = rooms.Count;
            FlatBuffers.FlatBufferBuilder builder = new FlatBuffers.FlatBufferBuilder(1);
            FlatBuffers.Offset<WorldMessages.Room>[] roomOffs = new FlatBuffers.Offset<WorldMessages.Room>[len];
            for (int i = 0; i < len; i++)
            {
                var item = rooms[i];
                var vec = WorldMessages.Room.CreateRoom(builder, builder.CreateString(item.Id), item.PlayerCount);
                roomOffs[i] = vec;
            }
            var roomListVec = GetRoomListReply.CreateGetRoomListReply(builder, GetRoomListReply.CreateRoomVector(builder, roomOffs));
            builder.Finish(roomListVec.Value);
            //Console.WriteLine("RoomLength: " + GetRoomListReply.GetRootAsGetRoomListReply(new FlatBuffers.ByteBuffer(Utility.GetDataBuffer(builder.DataBuffer.Length - builder.DataBuffer.Position, builder.DataBuffer.Get, builder.DataBuffer.Position), 0)).RoomLength);

            FlatBuffers.FlatBufferBuilder wBuilder = new FlatBuffers.FlatBufferBuilder(1);
            var worldVec = ReplyMsg.CreateReplyMsg(
                wBuilder, 
                MessageType.GetRoomListReply, 
                msg.MsgId,
                ReplyMsg.CreateBuffVector(wBuilder, Utility.GetDataBuffer(builder.DataBuffer.Length - builder.DataBuffer.Position, builder.DataBuffer.Get, builder.DataBuffer.Position))
                );
            wBuilder.Finish(worldVec.Value);
            session.Send(wBuilder.DataBuffer.Data, wBuilder.DataBuffer.Position, wBuilder.DataBuffer.Length - wBuilder.DataBuffer.Position);
        }
    }
}
