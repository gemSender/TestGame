using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;
using world_messages;
using Google.ProtocolBuffers;

namespace TaskTest.ServerFramework
{
    public class WorldRequest : UdpRequestInfo
    {
        public WorldMessage msg
        {
            get;
            private set;
        }
        public WorldRequest(MessageType keyEnum, string sessionId) : base(keyEnum.ToString(), sessionId)
        { 
            
        }

        public static WorldRequest Decode(byte[] buff, int offset, int length)
        {
            WorldMessage msg = WorldMessage.ParseFrom(ByteString.CopyFrom(buff, offset, length));
            return new WorldRequest(msg.Type, msg.PlayerId) { msg = msg};
        }
    }
}
