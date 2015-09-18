using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TaskTest.ServerFramework
{
    public class GameSession : SuperSocket.SocketBase.AppSession<GameSession, UserUdpRequest>
    {
        public byte[] dataBuff = new byte[1024];

        public bool Reply(ValueType data)
        {
            int len1 = Utility.WriteStructToBuffer(MessageType.RpcReply, dataBuff, 0);
            int len2 = Utility.WriteStructToBuffer(data, dataBuff, len1);
            return TrySend(dataBuff, 0, len1 + len2);
        }

        public bool Rpc(GameCommand.UserCommandType command, ValueType arg)
        {
            int len1 = Utility.WriteStructToBuffer(MessageType.RpcCall, dataBuff, 0);
            int len2 = Utility.WriteStructToBuffer(command, dataBuff, len1);
            int len3 = Utility.WriteStructToBuffer(arg, dataBuff, len2);
            return TrySend(dataBuff, 0, len1 + len2 + len3);
        }
    }
}
