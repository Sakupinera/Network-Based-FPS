namespace GameServer
{
    public class RoomListMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            RoomListMsg msg = message as RoomListMsg;
            //EventCenter.GetInstance().EventTrigger("RefreshRoomList", msg);
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<RoomListMsg>.Create(msg));

        }
    }
}
