namespace GameServer
{
    public class RoomInfoMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            RoomInfoMsg msg = message as RoomInfoMsg;
            EventCenter.GetInstance().EventTrigger("RefreshPlayerList", msg);
        }
    }
}
