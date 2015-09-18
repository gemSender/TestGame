using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TaskTest.ServerFramework;

namespace TaskTest.Game
{
    public class World
    {
        static World mInstance;
        public Action onDestroy;
        Queue<Tuple<Func<object>, Action<GameSession, object>, GameSession>> actions = new Queue<Tuple<Func<object>,Action<GameSession, object>, GameSession>>();
        public static World Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new World();
                }
                return mInstance;
            }
        }

        public static readonly float sleepTime = 1 / 120f;
        public float time
        {
            get
            {
                return sw.ElapsedMilliseconds / 1000f;
            }
        }
        int _nextId = 0;
        Stopwatch sw;
        bool destroyed;

        public bool IsDestroyed
        {
            get
            {
                return destroyed;
            }
        }
        public int NextId
        {
            get
            {
                return _nextId++;
            }
        }
        List<Room> rooms = new List<Room>();
        public Room CreateRoom(int pId)
        {
            var newRoom = new Room(NextId);
            rooms.Add(newRoom);
            newRoom.AddPlayer(pId);
            Console.WriteLine("Room {0} Created", newRoom.rId);
            return newRoom;
        }

        public void MainLoop()
        {
            while (!destroyed)
            {
                lock (actions)
                {
                    while (actions.Count > 0)
                    {
                        var action = actions.Dequeue();
                        var ret = action.Item1();
                        if (action.Item2 != null)
                        {
                            action.Item2(action.Item3, ret);
                        }
                    }
                }
                for (int i = 0; i < rooms.Count; i++)
                {
                    var room = rooms[i];
                    if (time - room.lastUpdateTime > Room.deltaTime)
                    {
                        room.Update(time);
                    }
                }
            }
            Console.WriteLine("World has been destoryed");
        }
        private World()
        {
            sw = new Stopwatch();
            sw.Start();
        }

        public void Destroy()
        {
            destroyed = true;
            if (onDestroy != null) {
                onDestroy();
            }
        }

        public void AddPlayer(int rId, int uId)
        {
            var item = rooms.Find(x => x.rId == rId);
            if (item != null)
            {
                item.AddPlayer(uId);
            }
        }


        public Room GetRoom(int rId)
        {
            return rooms.Find(x => x.rId == rId);
        }

        public void InvokeAction<RetType>(Func<RetType> action, Action<GameSession, RetType> callBack = null, GameSession session = null)
        {
            lock (actions)
            {
                Func<object> finalAction = () => action();
                if (callBack != null)
                {
                    actions.Enqueue(new Tuple<Func<object>, Action<GameSession, object>, GameSession>(finalAction, (sess, o) => callBack(sess, (RetType)o), session));
                }
                else {
                    actions.Enqueue(new Tuple<Func<object>, System.Action<GameSession, object>, GameSession>(finalAction, null, session));
                }
            }
        }
    }
}
