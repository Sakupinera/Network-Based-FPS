namespace GamePlayer
{
    public class ChangeCampMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            ChangeCampMsg msg = message as ChangeCampMsg;
            RoomMsgLogic.ChangeCamp(GameServer.ServerSocket.GetInstance().GetClient(msg.id).player, msg.CAMP);
        }
    }
}
