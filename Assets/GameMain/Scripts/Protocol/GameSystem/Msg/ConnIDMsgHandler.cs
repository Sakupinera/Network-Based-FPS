using UnityEngine;

namespace GameSystem
{
    public class ConnIDMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            ConnIDMsg msg = message as ConnIDMsg;
            //NetMgr.Instance.ID = msg.id;
            NetworkBasedFPS.GameEntry.Net.ID = msg.id;
            Debug.Log("客户端ID：" + msg.id);
            NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<ConnIDMsg>.Create(msg));
        }
    }
}
