namespace GameCommand
{
    public struct RoomPlayerHead
    {
        public int pId;
        public int rId;
    }
    public struct CreateRoom
    {
        public int pId;
        public int capacity;
    }

    public struct StartGame
    {
        public RoomPlayerHead head;
    }

    public struct Jump
    {
        public RoomPlayerHead head;
    }
}