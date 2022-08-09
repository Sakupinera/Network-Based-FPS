using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class RoomListForm : UGuiForm
    {

        [SerializeField]
        private Button m_ButtonCreate;
        [SerializeField]
        private Button m_ButtonRefresh;
        [SerializeField]
        private Button m_ButtonBack;
        [SerializeField]
        private Button m_ButtonSetting;
        [SerializeField]
        private GameObject m_RoomItem;
        [SerializeField]
        private GameObject m_Content;


        public void OnBackButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
            Close(true);
        }

        public void OnRefreshButtonClick()
        {
            GetRoomListMsg msg = new GetRoomListMsg();
            msg.id = GameEntry.Net.ID;
            GameEntry.Net.Send(msg);
        }

        //刷新房间列表
        public void RefreshRoomList(object sender, GameEventArgs e)
        {
            MsgEventArgs<RoomListMsg> msgEventArgs = (MsgEventArgs<RoomListMsg>)e;
            RoomListMsg msg = msgEventArgs.Msg;
            print("刷新房间列表");
            m_Content.BroadcastMessage("DestroyMySelf", SendMessageOptions.DontRequireReceiver);

            for (int i = 0; i < msg.roomList.Count; i++)
            {
                GameObject rp = Instantiate(m_RoomItem, m_Content.transform);
                rp.GetComponent<RoomItem>().SetText(i + 1, msg.roomList[i].roomPlayersNum, msg.roomList[i].roomStatus);
            }
        }


        private void OnCreateButtonClick()
        {
            CteateRoomMsg msg = new CteateRoomMsg();
            msg.id = GameEntry.Net.ID;
            GameEntry.Net.Send(msg);
            GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
            Close(true);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_ButtonBack.onClick.AddListener(OnBackButtonClick);
            m_ButtonCreate.onClick.AddListener(OnCreateButtonClick);
            m_ButtonRefresh.onClick.AddListener(OnRefreshButtonClick);
        }


        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            GameEntry.Event.Subscribe(MsgEventArgs<RoomListMsg>.EventId, RefreshRoomList);
        }


        protected override void OnClose(bool isShutdown, object userData)
        {

            GameEntry.Event.Unsubscribe(MsgEventArgs<RoomListMsg>.EventId, RefreshRoomList);

            base.OnClose(isShutdown, userData);
        }
    }



}
