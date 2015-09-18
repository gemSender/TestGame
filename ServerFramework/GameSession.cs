using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTest.ServerFramework
{
    public class GameSession : SuperSocket.SocketBase.AppSession<GameSession, UserUdpRequest>
    {
        public byte[] dataBuff = new byte[1024];

        public void Send(ValueType data)
        {
            int len = Utility.WriteStructToBuffer(data, dataBuff, 0);
            TrySend(dataBuff, 0, len);
        }
    }
}
