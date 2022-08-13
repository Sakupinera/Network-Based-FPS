namespace GamePlayer
{
    public class ShootMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            ShootMsg msg = message as ShootMsg;
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<ShootMsg>.Create(msg));
        }
    }
}
