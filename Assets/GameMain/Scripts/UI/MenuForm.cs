using GameFramework.Event;
using GamePlayer;
using GameServer;
using GameSystem;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    public class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu = null;

        [SerializeField]
        private Button m_StartButton = null;

        [SerializeField]
        private Button m_SettingButton = null;

        [SerializeField]
        private Button m_ConnButton = null;

        [SerializeField]
        private Button m_QuitButton = null;

        [SerializeField]
        private InputField m_IPInputField = null;

        [SerializeField]
        private InputField m_PortInputField = null;

        [SerializeField]
        private InputField m_PlayerIDInputField = null;

        public void OnSettingButtonClick()
        {
            print("设置");
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnStartButtonClick()
        {
            print("开始");

            if (GameEntry.Net == null || GameEntry.Net.ID == 0)
            {
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "未连接服务器");
            }
            else if (m_PlayerIDInputField.text == "")
            {
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "玩家ID未填写");
            }
            else
            {
                Login();
                //GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/RoomListForm.prefab", "Default");
                GameEntry.UI.OpenUIForm(UIFormId.RoomListForm);
                Close(true);
            }
        }

        public void OnConnButtonClick()
        {
            print("连接");
            int port = 8080;
            if (int.TryParse(m_PortInputField.text, out port))
            {
                GameEntry.Net.Connect(m_IPInputField.text, port);
            }
        }

        public void OnQuitButtonClick()
        {
            Application.Quit();
        }



        public void Login()
        {
            PlayerInfoMsg playerInfoMsg = new PlayerInfoMsg();
            playerInfoMsg.playerID = GameEntry.Net.ID;
            playerInfoMsg.data = new GamePlayer.PlayerData();
            playerInfoMsg.data.name = m_PlayerIDInputField.text;
            playerInfoMsg.data.id = GameEntry.Net.ID;
            playerInfoMsg.data.playerCamp = E_PLAYER_CAMP.A;
            GameEntry.Net.Send(playerInfoMsg);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StartButton.onClick.AddListener(OnStartButtonClick);
            m_SettingButton.onClick.AddListener(OnSettingButtonClick);
            m_ConnButton.onClick.AddListener(OnConnButtonClick);
            m_QuitButton.onClick.AddListener(OnQuitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);


            GameEntry.Event.Subscribe(MsgEventArgs<ConnIDMsg>.EventId, ShowPop);

        }

        private void ShowPop(object sender, GameEventArgs e)
        {
            MsgEventArgs<ConnIDMsg> msgEventArgs = (MsgEventArgs<ConnIDMsg>)e;
            ConnIDMsg msg = msgEventArgs.Msg;
            if (msg.id != 0)
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "连接成功");
            else
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "连接出错");

        }

        protected override void OnClose(bool isShutdown, object userData)
        {


            GameEntry.Event.Unsubscribe(MsgEventArgs<ConnIDMsg>.EventId, ShowPop);

            base.OnClose(isShutdown, userData);
        }
    }

}
