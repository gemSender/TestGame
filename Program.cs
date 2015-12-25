using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TaskTest.Game;
using SuperSocket.SocketBase;
using System.Net;
using System.Net.Sockets;
using SuperSocket.SocketBase.Config;
using TaskTest.ServerFramework;
using SuperSocket.SocketEngine;

namespace TaskTest
{
    class Program
    {
        /*
        static void Main(string[] args)
        {
            World world = World.Instance;
            Task gameTask = new Task(world.MainLoop);
            IO.MessageReceiver.Instance.Init();
            Task netTask = new Task(IO.MessageReceiver.Instance.RecvLoop);
            gameTask.Start();
            netTask.Start();
            while(!world.IsDestroyed)
            {
                string command = Console.ReadLine();
                switch (command) 
                { 
                    case "cr":
                        world.InvokeAction(() => world.CreateRoom(1));
                        break;
                    case "dw":
                        world.Destroy();
                        break;
                    case "ap":
                        Console.WriteLine("Input RoomId and PlayerId");
                        string idStr = Console.ReadLine();
                        string[] r_u = idStr.Split(' ');
                        if (r_u.Length == 2)
                        {
                            int rId, uId;
                            if (int.TryParse(r_u[0], out rId) && int.TryParse(r_u[1], out uId))
                            {
                                world.InvokeAction(() => world.AddPlayer(rId, uId));
                            }
                        }
                        break;
                }
            }

            gameTask.Wait();
            netTask.Wait();
        }
         * */

        static void StartWithConfig()
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey();
            Console.WriteLine();
            var bootstrap = BootstrapFactory.CreateBootstrap();

            if (!bootstrap.Initialize())
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey();
                return;
            }

            var result = bootstrap.Start();

            Console.WriteLine("Start result: {0}!", result);

            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }

            Console.WriteLine();

            //Stop the appServer
            bootstrap.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
        static void Main(string[] args) 
        {
            StartWithConfig();
        }

    }
}
