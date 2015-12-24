using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;
using Messages;
using FlatBuffers;

namespace TaskTest.ServerFramework.World
{
    class UserWorldRequest : UdpRequestInfo
    {
        
        public UserWorldRequest(string key, string sessionId) : base(key, sessionId)
        {
            
        }

        public static UserWorldRequest Decode(byte[] src, int offset, int length)
        { 

        }
    }
}
