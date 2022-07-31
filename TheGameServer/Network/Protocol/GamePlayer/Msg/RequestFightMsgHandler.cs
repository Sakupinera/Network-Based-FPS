namespace GamePlayer
{
    public class RequestFightMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            RequestFightMsg msg = message as RequestFightMsg;
            BattleMsgLogic.MsgStartFight(GameServer.ServerSocket.GetInstance().GetClient(msg.id).player);
        }
    }
}
