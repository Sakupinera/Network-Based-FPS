namespace GamePlayer
{
    public class MoveMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            MoveMsg msg = message as MoveMsg;
            //UnityEngine.Debug.Log(msg.playerPos.id + " " + msg.playerPos.posY + " " + msg.playerPos.posY + " " + msg.playerPos.posZ);
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<MoveMsg>.Create(msg));
        }
    }
}
