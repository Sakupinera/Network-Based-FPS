namespace GameServer
{
    public class RoomListMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            RoomListMsg msg = message as RoomListMsg;
            EventCenter.GetInstance().EventTrigger("RefreshRoomList", msg);
        }
    }
}
