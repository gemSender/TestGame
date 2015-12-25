using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TaskTest.ServerFramework
{
    public class WorldServer : AppServer<WorldSession, WorldRequest>
    {
        public WorldServer()
            : base(new DefaultReceiveFilterFactory<WorldReceiverFilter, WorldRequest>())
        {

        }
    }
}
