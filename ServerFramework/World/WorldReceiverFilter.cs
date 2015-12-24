using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace TaskTest.ServerFramework
{
    class WorldReceiverFilter : IReceiveFilter<WorldRequest>
    {

        public WorldRequest Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            return WorldRequest.Decode(readBuffer, offset, length);
        }

        public int LeftBufferSize
        {
            get { return 0; }
        }

        public IReceiveFilter<WorldRequest> NextReceiveFilter
        {
            get { return this; }
        }

        public void Reset()
        {
        }

        public FilterState State
        {
            get;
            private set;
        }
    }
}
