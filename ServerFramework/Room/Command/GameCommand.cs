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
            World.Instance.ProcessCommand(session, msg);
        }
    }

    public class ReadyForGame : RoomMessage{ }
    public class Rpc : RoomMessage{ }

    public class CreateObj : RoomMessage{}

    //public class AddPlayer : RoomMessage
    //{
    //    public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
    //    {
    //        var msg = requestInfo.msg;
    //        var arrSeg = msg.GetBufBytes().Value;
    //        var roomId = System.Text.Encoding.UTF8.GetString(arrSeg.Array, arrSeg.Offset, arrSeg.Count);
    //        Console.WriteLine("Player {0} Enter Room {1}", session.SessionID, roomId);
    //        World.Instance.EnterRoom(session, roomId);
    //    }
    //}

    public class GetMissingCmd : CommandBase<GameSession, UserUdpRequest> 
    {

        public override void ExecuteCommand(GameSession session, UserUdpRequest requestInfo)
        {
            var msg = requestInfo.msg;
            Room room = World.Instance.GetRoomByPlayerId(session.SessionID);
            var buf = msg.Buf;
            var playerId = buf.ToStringUtf8();//System.Text.Encoding.UTF8.GetString(bufSeg.Array, bufSeg.Offset, bufSeg.Count);
            if (room != null) {
                lock (room) {
                    var targetCmd = room.GetCommand(playerId, msg.Frame);
                    if (targetCmd != null) {
                        session.Send(targetCmd);
                    }
                }
            }
        }
    }

    public class Empty : RoomMessage { }
}

