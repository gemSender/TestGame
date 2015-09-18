using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTest.ServerFramework;
namespace TaskTest.Game
{
    public class Player
    {
        public int id;
        public GameSession session;
        public float height;
        public float speed;
        public bool dead;
        public bool Jump()
        {
            if (dead)
                return false;
            speed = 5f;
            return true;
        }
    }
}
