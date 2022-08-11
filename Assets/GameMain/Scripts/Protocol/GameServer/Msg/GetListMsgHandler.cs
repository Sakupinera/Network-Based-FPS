namespace GameServer
{
    public class GetListMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            GetListMsg msg = message as GetListMsg;
            UnityEngine.Debug.Log("收到开战消息");
            NetworkBasedFPS.GameEntry.Event.Fire(this, MsgEventArgs<GetListMsg>.Create(msg));
        }
    }
}
