namespace GameCommand
{
    public struct CreateRoom
    {
        public int pId;
        public int capacity;
    }

    public struct StartGame
    {
        public int pId;
        public int rId;
    }
}