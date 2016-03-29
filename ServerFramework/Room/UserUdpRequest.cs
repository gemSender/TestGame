using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;
using messages;
using Google.ProtocolBuffers;

namespace TaskTest.ServerFramework
{
    public class UserUdpRequest : UdpRequestInfo
    {
        public messages.GenMessage msg
        {
            get;
            private set;
        }
        public UserUdpRequest(MessageType msgType, string sessionID) :base(msgType.ToString(), sessionID){
            
        }

        public static UserUdpRequest Decode(byte[] src, int offset, int length)
        {
            var msg = GenMessage.ParseFrom(ByteString.CopyFrom(src, offset, length));
            return new UserUdpRequest(msg.MsgType, msg.PId) { msg = msg};
        }
        
        
    }
}
