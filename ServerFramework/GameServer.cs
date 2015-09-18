using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TaskTest.ServerFramework
{
    class GameServer : AppServer<GameSession, UserUdpRequest>
    {
        public GameServer() : base(new DefaultReceiveFilterFactory<GameReceiverFilter, UserUdpRequest>())
        {

        }
    }

    public class PythonAppServer : AppServer
    {
        public PythonAppServer() : base()
        {
            this.NewSessionConnected += (session) =>
            {
                session.Send("Welcome");
            };
        }
    }
}
