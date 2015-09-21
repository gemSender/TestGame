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

    public struct RoomRpcNoArg
    {
        public RoomPlayerHead head;
    }

}