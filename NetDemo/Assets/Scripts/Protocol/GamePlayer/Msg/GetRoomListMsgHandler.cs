namespace GamePlayer
{
    public class GetRoomListMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            GetRoomListMsg msg = message as GetRoomListMsg;
        }
    }
}
