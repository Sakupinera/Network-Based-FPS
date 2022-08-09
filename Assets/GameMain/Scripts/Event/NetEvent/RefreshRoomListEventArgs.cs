using GameFramework;
using GameFramework.Event;
using GameServer;

public class RefreshRoomListEventArgs : MsgEventArgs<RoomListMsg>
{
    //事件编号
    public static readonly int EventId = typeof(RefreshRoomListEventArgs).GetHashCode();

    //ID
    public override int Id
    {
        get { return EventId; }
    }

    public string connInfo
    {
        get;
        private set;
    }

    //public static conneventargs create(string info)
    //{
    //    //conneventargs conneventargs = referencepool.acquire<conneventargs>();
    //    //conneventargs.conninfo = info;
    //    //return conneventargs;
    //}

    public override void Clear()
    {
        connInfo = null;
    }
}
