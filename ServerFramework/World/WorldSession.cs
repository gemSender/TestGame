using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using world_messages;
using Google.ProtocolBuffers;
using TaskTest.Utility;

namespace TaskTest.ServerFramework
{
    public class WorldSession : AppSession<WorldSession, WorldRequest>
    {
        public void Reply(MessageType type, int msgId, ArraySegment<byte> bufferSeg)
        {
            /*
            FlatBufferBuilder fb = new FlatBufferBuilder(1);
            var vec = ReplyMsg.CreateReplyMsg(fb, type, msgId, fb.CreateBuffVector(ReplyMsg.StartBuffVector, bufferSeg));
            fb.Finish(vec.Value);
             * */
            var msg = ReplyMsg.CreateBuilder()
                .SetType(type)
                .SetMsgId(msgId)
                .SetBuff(ByteString.CopyFrom(bufferSeg.Array, 0, bufferSeg.Count)).Build();
            var bytes = msg.ToByteArray();
            Send(bytes, 0, bytes.Length);
        }
    }
}
