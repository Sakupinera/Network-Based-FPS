namespace GameServer
{
    public class RoomInfoMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            RoomInfoMsg msg = message as RoomInfoMsg;
            UnityEngine.Debug.Log("收到房间消息");
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<RoomInfoMsg>.Create(msg));
        }
    }
}
