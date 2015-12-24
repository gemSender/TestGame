using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;
using WorldMessages;
using FlatBuffers;

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
            ByteBuffer bb = new ByteBuffer(buff, offset);
            WorldMessage msg = WorldMessage.GetRootAsWorldMessage(bb);
            return new WorldRequest(msg.Type, msg.PlayerId) { msg = msg};
        }
    }
}
