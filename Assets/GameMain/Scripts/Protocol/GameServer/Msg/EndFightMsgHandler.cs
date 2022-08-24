namespace GameServer
{
    public class EndFightMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            EndFightMsg msg = message as EndFightMsg;
            NetworkBasedFPS.GameEntry.Event.Fire(this, MsgEventArgs<EndFightMsg>.Create(msg));
        }
    }
}
