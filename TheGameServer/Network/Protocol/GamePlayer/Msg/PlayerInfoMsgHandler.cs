namespace GamePlayer
{
    public class PlayerInfoMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            PlayerInfoMsg msg = message as PlayerInfoMsg;
            PlayerMsgLogic.Login(msg.playerID, msg.data);

        }
    }
}
