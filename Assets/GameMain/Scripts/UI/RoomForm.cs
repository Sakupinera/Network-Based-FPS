using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class RoomForm : UGuiForm
    {
        [SerializeField]
        private Button m_LeaveButton = null;

        [SerializeField]
        private Button m_ReadyButton = null;

        [SerializeField]
        private Button m_CampButton_l = null;
        [SerializeField]
        private Button m_CampButton_r = null;

        [SerializeField]
        private GameObject content = null;

        [SerializeField]
        private GameObject playerItem = null;

        [SerializeField]
        private GameObject camp_1 = null;
        [SerializeField]
        private GameObject camp_2 = null;


        private E_PLAYER_CAMP nowCamp;
        private bool isOner = false;


        public void RequestRoomInfo()
        {
            GetRoomInfoMsg msg = new GetRoomInfoMsg();
            msg.id = GameEntry.Net.ID;
            GameEntry.Net.Send(msg);
        }

        private void RefreshPlayerList(object sender, GameEventArgs e)
        {
            MsgEventArgs<RoomInfoMsg> msgEventArgs = (MsgEventArgs<RoomInfoMsg>)e;
            RoomInfoMsg msg = msgEventArgs.Msg;
            print("刷新玩家列表");
            content.BroadcastMessage("DestroyMySelf", SendMessageOptions.DontRequireReceiver);

            //是否为房主
            if (msg.Oner == GameEntry.Net.ID)
                isOner = true;
            else
                isOner = false;
            for (int i = 0; i < msg.roomPlayersList.Count; i++)
            {
                //玩家信息
                GameObject pip = Instantiate(playerItem, content.transform);
                pip.GetComponent<PlayerItem>().SetText(msg.roomPlayersList[i].name, msg.roomPlayersList[i].id == msg.Oner, msg.roomPlayersList[i].playerCamp);
                if (msg.roomPlayersList[i].id == GameEntry.Net.ID)
                {
                    nowCamp = msg.roomPlayersList[i].playerCamp;
                }
            }
            float visible = 1;
            float invisible = 0;
            if (nowCamp == E_PLAYER_CAMP.A)
            {
                camp_1.GetComponent<CanvasGroup>().alpha = visible;
                camp_2.GetComponent<CanvasGroup>().alpha = invisible;
            }
            else if (nowCamp == E_PLAYER_CAMP.B)
            {
                camp_1.GetComponent<CanvasGroup>().alpha = invisible;
                camp_2.GetComponent<CanvasGroup>().alpha = visible;
            }
        }

        //private void Update()
        //{

        //}

        //离开房间
        public void LeaveRoom()
        {
            print("离开房间");
            LeaveRoomMsg msg = new LeaveRoomMsg();
            msg.id = GameEntry.Net.ID;
            GameEntry.Net.Send(msg);
            GameEntry.UI.OpenUIForm(UIFormId.RoomListForm);
            Close(true);
        }

        //切换阵营
        public void ChangeCamp()
        {
            print("当前阵营" + nowCamp.ToString());
            ChangeCampMsg msg = new ChangeCampMsg();
            msg.id = GameEntry.Net.ID;
            if (nowCamp == E_PLAYER_CAMP.A)
                msg.CAMP = E_PLAYER_CAMP.B;
            if (nowCamp == E_PLAYER_CAMP.B)
                msg.CAMP = E_PLAYER_CAMP.A;
            GameEntry.Net.Send(msg);
        }

        //开始游戏
        public void StartGame()
        {
            //不是房主则显示弹窗 请等待房主开始游戏
            if (!isOner)
            {
                //显示弹窗
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "不是房主,不能开启战斗");
                print("不是房主 不能开启战斗");
                return;
            }
            print("请求开始战斗");
            //发送开始战斗协议
            RequestFightMsg msg = new RequestFightMsg();
            msg.id = GameEntry.Net.ID;
            GameEntry.Net.Send(msg);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_LeaveButton.onClick.AddListener(LeaveRoom);
            m_CampButton_l.onClick.AddListener(ChangeCamp);
            m_CampButton_r.onClick.AddListener(ChangeCamp);
            m_ReadyButton.onClick.AddListener(StartGame);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(MsgEventArgs<RoomInfoMsg>.EventId, RefreshPlayerList);


            RequestRoomInfo();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            print("取消监听");
            GameEntry.Event.Unsubscribe(MsgEventArgs<RoomInfoMsg>.EventId, RefreshPlayerList);

            base.OnClose(isShutdown, userData);
        }
    }
}

