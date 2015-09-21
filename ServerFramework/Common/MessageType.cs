using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Common
{
    enum MessageType
    {
        RpcReply,
        RpcCall
    }

    public struct EnterRoomReply
    {
        public int count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public int[] ids;
    }
}
