namespace GamePlayer
{
    public class DamageMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            DamageMsg msg = message as DamageMsg;
            //msg.injured 的HP - msg.damage
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<DamageMsg>.Create(msg));
        }
    }
}
