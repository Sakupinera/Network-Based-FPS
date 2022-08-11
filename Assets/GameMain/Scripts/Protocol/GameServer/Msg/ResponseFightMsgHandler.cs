
using UnityEngine;

namespace GameServer
{
    public class ResponseFightMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            ResponseFightMsg msg = message as ResponseFightMsg;
            switch (msg.responseFight)
            {
                case 1:
                    Debug.Log("开启战斗成功");
                    //EventCenter.GetInstance().EventTrigger("LoadingBattleScene");
                    NetworkBasedFPS.GameEntry.Event.FireNow(this, MsgEventArgs<ResponseFightMsg>.Create(msg));
                    break;
                case -1:
                    Debug.Log("开启战斗失败 玩家状态有误");
                    break;
                case -2:
                    Debug.Log("开启战斗失败 玩家不是房主");
                    break;
                case -3:
                    Debug.Log("开启战斗失败 未达到开战条件");
                    break;
            }
        }
    }
}
