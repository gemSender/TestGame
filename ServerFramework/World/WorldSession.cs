using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using WorldMessages;
using FlatBuffers;
using TaskTest.Utility;

namespace TaskTest.ServerFramework
{
    public class WorldSession : AppSession<WorldSession, WorldRequest>
    {
        public void Reply(MessageType type, int msgId, ArraySegment<byte> bufferSeg)
        {
            FlatBufferBuilder fb = new FlatBufferBuilder(1);
            var vec = ReplyMsg.CreateReplyMsg(fb, type, msgId, fb.CreateBuffVector(ReplyMsg.StartBuffVector, bufferSeg));
            fb.Finish(vec.Value);
            Send(fb.DataBuffer.GetArraySegment());
        }
    }
}
