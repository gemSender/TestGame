using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;
using GameCommand;

namespace TaskTest.ServerFramework
{
    public class UserUdpRequest : UdpRequestInfo
    {
        byte[] dataBuf;

        public byte[] DataStream
        {
            get {
                return dataBuf;
            }
        }

        public string StringData {
            get
            {
                return Encoding.UTF8.GetString(dataBuf);
            }
        }
        public UserUdpRequest(string key, string sessionID) :base(key, sessionID){
            
        }

        public UserUdpRequest SetValue(byte[] src, int startIndex, int length)
        {
            dataBuf = new byte[length];
            Buffer.BlockCopy(src, startIndex, dataBuf, 0, length);
            Console.WriteLine("key : {0}, session : {1}, valueStr : {2}", Key, SessionID, Encoding.UTF8.GetString(dataBuf));
            return this;
        }

        public static UserUdpRequest Decode(byte[] src, int offset, int length)
        {
            UserCommandType keyEnum = Utility.ByteToStruct<UserCommandType>(src, offset);
            var key = keyEnum.ToString();
            if (keyEnum == UserCommandType.GetSession)
            {
                return new UserUdpRequest(key, Guid.NewGuid().ToString());
            }
            else {
                if (length <= 40)
                {
                    return null;
                }
                var sessionId = Encoding.UTF8.GetString(src, 4 + offset, 36);
                return new UserUdpRequest(key, sessionId).SetValue(src, 40 + offset, length - 40);
            }
        }
    }
}
