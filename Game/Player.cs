using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTest.ServerFramework;
using Messages;
namespace TaskTest.Game
{
    public class Player
    {
        public const int maxCmdCount = 16;
        public int nextMsgId;
        public string id;
        public GameSession session;
        public int lastFrame;
        public List<Messages.GenMessage> commands = new List<Messages.GenMessage>();
        public bool GetCommand(GenMessage msg)
        {
            int i = 0;
            for (i = commands.Count - 1; i >= 0; i--) {
                var item = commands[i];
                int diff = item.Frame - msg.Frame;
                if (diff < 0) {
                    commands.Insert(i + 1, msg);
                    return true;
                }
                else if (diff == 0) {
                    Console.Write("StoredFrames: ");
                    foreach (var m in commands) {
                        Console.Write(m.Frame + " ");
                    }
                    Console.WriteLine("SameFrame: " + msg.Frame + " MsgId: " + item.MsgId + " " + msg.MsgId);
                    return false;
                }
            }
            if (i == -1) {
                commands.Insert(0, msg);
                return true;
            }
            return false;
        }

        public void TryRemoveHead()
        {
            if (commands.Count > maxCmdCount)
            {
                commands.RemoveAt(0);
            }
        }
    }
}
