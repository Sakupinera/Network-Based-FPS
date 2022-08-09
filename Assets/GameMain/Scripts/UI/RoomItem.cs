using GamePlayer;
using GameServer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class RoomItem : UGuiForm
    {
        [SerializeField]
        private CanvasGroup m_BGImage_Prepare;
        [SerializeField]
        private CanvasGroup m_BGImage_Fight;
        [SerializeField]
        private Text m_RoomNoText;
        [SerializeField]
        private Text m_NumText;
        [SerializeField]
        private Button m_EnterButton;

        private int roomNo;
        private int num;
        private E_ROOM_STATUS status;

        public void SetText(int roomNo, int num, E_ROOM_STATUS status)
        {
            this.num = num;
            this.status = status;
            m_NumText.text = num.ToString() + "/10";
            if (status == E_ROOM_STATUS.PREPARE)
            {

                m_BGImage_Fight.alpha = 0;
            }
            else
            {
                m_BGImage_Prepare.alpha = 0;
            }
            this.roomNo = roomNo;

            m_EnterButton.onClick.AddListener(EnterRoom);
        }

        public void EnterRoom()
        {
            if (status == E_ROOM_STATUS.FIGHT)
            {
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "该房间正在战斗");
                return;
            }
            if (num == 10)
            {
                GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/PopForm.prefab", "Pop", "房间人数已满");
                return;
            }
            print("加入房间，序号为：" + roomNo);

            EnterRoomMsg msg = new EnterRoomMsg();
            msg.id = GameEntry.Net.ID;
            msg.index = roomNo;
            GameEntry.Net.Send(msg);

            GameEntry.UI.OpenUIForm(UIFormId.RoomForm);
        }

        public void DestroyMySelf()
        {
            Destroy(gameObject);
        }
    }

}
