using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using FlatBuffers;
using TaskTest.Game;

namespace TaskTest.ServerFramework
{
    public class GameSession : SuperSocket.SocketBase.AppSession<GameSession, UserUdpRequest>
    {
        public bool Send(ByteBuffer bb)
        {
            return TrySend(bb.Data, bb.Position, bb.Length - bb.Position);
        }
    }
}
