using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;
using Messages;
using FlatBuffers;

namespace TaskTest.ServerFramework
{
    public class UserUdpRequest : UdpRequestInfo
    {
        public Messages.GenMessage msg
        {
            get;
            private set;
        }
        public UserUdpRequest(string key, string sessionID) :base(key, sessionID){
            
        }

        public static UserUdpRequest Decode(byte[] src, int offset, int length)
        {
            ByteBuffer bb = new ByteBuffer(src, 0);
            Messages.GenMessage msg = Messages.GenMessage.GetRootAsGenMessage(bb);
            return new UserUdpRequest("GenMessage", msg.PId) { msg = msg};
        }
        
        
    }
}
