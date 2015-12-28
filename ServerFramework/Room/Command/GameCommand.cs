using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase;
using TaskTest.Game;

namespace TaskTest.ServerFramework
{

    public class RoomMessage : CommandBase<GameSession, UserUdpRequest> 
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var msg = requestInfo.msg;
            World.Instance.ProcessCommand(session, msg);
        }
    }

    public class Rpc : RoomMessage{ }

    public class CreateObj : RoomMessage{}

    public class AddPlayer : RoomMessage
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            World.Instance.EnterRoom(session, requestInfo.SessionID);
        }
    }

    public class Empty : RoomMessage { }
}

