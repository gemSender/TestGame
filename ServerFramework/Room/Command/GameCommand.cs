using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase;
using TaskTest.Game;
using System.Threading;

namespace TaskTest.ServerFramework
{

    public class RoomMessage : CommandBase<GameSession, UserUdpRequest> 
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var msg = requestInfo.msg;
            //Console.WriteLine("MsgType: {0}, Frame: {1}, CurrentThread: {2}", msg.MsgType, msg.Frame, Thread.CurrentThread.ManagedThreadId);
            World.Instance.ProcessCommand(session, msg);
        }
    }

    public class Rpc : RoomMessage{ }

    public class CreateObj : RoomMessage{}

    public class AddPlayer : RoomMessage
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            Console.WriteLine("EnterRoom");
            var msg = requestInfo.msg;
            var arrSeg = msg.GetBufBytes().Value;
            var roomId = System.Text.Encoding.UTF8.GetString(arrSeg.Array, arrSeg.Offset, arrSeg.Count);
            World.Instance.EnterRoom(session, roomId);
        }
    }

    public class Empty : RoomMessage { }
}

