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
    using GameCommand_StartGame = GameCommand.RoomRpcNoArg;
    using GameCommand_Jump = GameCommand.RoomRpcNoArg;
    using GameCommand_EnterRoom = GameCommand.RoomRpcNoArg;
    public abstract class GameCommandBase<T> : CommandBase<GameSession, UserUdpRequest> where T : struct
    { 
        protected T GetCommandStruct(UserUdpRequest req)
        {
            return Utility.ByteToStruct<T>(req.DataStream, 0);
        }
    }
    public class CreateRoom : GameCommandBase<GameCommand.CreateRoom>
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var cmd = GetCommandStruct(requestInfo);
            World.Instance.InvokeAction(
                () => World.Instance.CreateRoom(cmd.pId),
                (sess, room) =>
                {
                    sess.Reply(room.rId);
                },
                session);
        }
    }

    public class StartGame : GameCommandBase<GameCommand_StartGame>
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var cmd = GetCommandStruct(requestInfo);
            var room = World.Instance.GetRoom(cmd.head.rId);
            World.Instance.InvokeAction(
                () => room.Start(),
                (sess, time) =>
                {
                    sess.Reply(time);
                },
                session);
        }
    }

    public class Jump : GameCommandBase<GameCommand_Jump>
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var cmd = GetCommandStruct(requestInfo);
            var room = World.Instance.GetRoom(cmd.head.rId);
            World.Instance.InvokeAction(
                () => room.Jump(cmd.head.pId),
                (sess, ret) => sess.Reply(ret),
                session
                );
        }
    }

    public class EnterRoom : GameCommandBase<GameCommand_EnterRoom>
    {
        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var cmd = GetCommandStruct(requestInfo);
            var room = World.Instance.GetRoom(cmd.head.rId);
            World.Instance.InvokeAction(
                () => room.EnterRoom(cmd.head.pId),
                (sess, ret) => sess.Reply(ret),
                session
                );
        }
    }

}
