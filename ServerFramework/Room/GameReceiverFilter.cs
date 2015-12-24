using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TaskTest.ServerFramework
{
    class GameReceiverFilter : IReceiveFilter<UserUdpRequest>
    {
        public FilterState State { get; private set; }
        public int LeftBufferSize
        {
            get { return 0; }
        }

        public IReceiveFilter<UserUdpRequest> NextReceiveFilter
        {
            get { return this; }
        }

        public void Reset()
        {

        }

        public UserUdpRequest Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            return UserUdpRequest.Decode(readBuffer, offset, length);
        }

    }
}
