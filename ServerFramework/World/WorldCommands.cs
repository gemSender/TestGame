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
    public class CreateRoom : CommandBase<GameSession, UserUdpRequest>
    {

    }
}
