namespace GamePlayer
{
    public class WeaponMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            WeaponMsg msg = message as WeaponMsg;
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<WeaponMsg>.Create(msg));
        }
    }
}
