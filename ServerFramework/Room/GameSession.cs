using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Google.ProtocolBuffers;
using TaskTest.Game;
using SuperSocket.SocketBase;

namespace TaskTest.ServerFramework
{
    public class GameSession : AppSession<GameSession, UserUdpRequest>
    {
        public bool Send(IMessage msg)
        {
            var bytes = msg.ToByteArray();
            return TrySend(bytes, 0, bytes.Length);
        }
    }
}
