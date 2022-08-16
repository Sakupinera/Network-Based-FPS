namespace GamePlayer
{
    public class StatusMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            StatusMsg msg = message as StatusMsg;
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<StatusMsg>.Create(msg));
        }
    }
}
