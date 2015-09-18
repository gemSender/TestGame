using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TaskTest.IO
{
    class GenMessage 
    {
        public int len;
        public byte[] data;
        public EndPoint endPoint;
        public GenMessage(int _len, byte[] _data, EndPoint endPoint)
        {
            this.len = _len;
            data = new byte[_len];
            Buffer.BlockCopy(_data, 0, data, 0, len);
            this.endPoint = endPoint;
        }
    }
    class MessageReceiver
    {
        Queue<GenMessage> msgBox = new Queue<GenMessage>();
        private MessageReceiver(){}

        public static readonly int port = 5610;
        Socket mSocket;
        static MessageReceiver mInstance;
        EndPoint remotePoint;
        public static MessageReceiver Instance {
            get {
                if (mInstance == null) {
                    mInstance = new MessageReceiver();
                }
                return mInstance;
            }
        }


        public void Init()
        {
            mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint ipLocalPoint = new IPEndPoint(0, port);
            mSocket.Bind(ipLocalPoint);
        }

        public void RecvLoop()
        {
            Game.World.Instance.onDestroy += StopRecvLoop;
            byte[] dataBuf = new byte[1024];
            remotePoint = new IPEndPoint(1, 0);
            while(!Game.World.Instance.IsDestroyed)
            {
                int len = mSocket.ReceiveFrom(dataBuf, ref remotePoint);
                
                if (len > 0)
                {
                    lock (msgBox)
                    {
                        Console.WriteLine(remotePoint.ToString());
                        msgBox.Enqueue(new GenMessage(len, dataBuf, remotePoint));
                    }
                }
            }
        }

        void StopRecvLoop()
        {
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            server.SendTo(new byte[0], 0, SocketFlags.None, new IPEndPoint(IPAddress.Loopback, port));
        }
    }
}
