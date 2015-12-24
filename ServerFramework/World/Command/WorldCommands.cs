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
    public class CreateRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            World.Instance.CreateRoom(session);
        }
    }

    public class EnterRoom : CommandBase<WorldSession, WorldRequest>
    {
        public override void ExecuteCommand(WorldSession session, WorldRequest requestInfo)
        {
            var msg = requestInfo.msg;
            var buf = msg.GetBuffBytes().Value.Array;
            string roomId = System.Text.Encoding.UTF8.GetString(buf);
            World.Instance.EnterRoom(session, roomId);
        }
    }
}
