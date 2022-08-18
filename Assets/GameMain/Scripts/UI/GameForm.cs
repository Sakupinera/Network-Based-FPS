using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class GameForm : UGuiForm
    {
        //小地图组件
        [SerializeField]
        private GameObject m_miniMap;

        //团队分数组件
        [SerializeField]
        private GameObject m_teamItem;

        //血条组件
        [SerializeField]
        private GameObject m_hpBarItem;

        //武器栏组件
        [SerializeField]
        private GameObject m_weaponBox;

        [SerializeField]
        private TMP_Text KillNum;

        private int m_killNum = 0;

        private int? miniMapId = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_miniMap.GetComponent<MiniMap>().Init();
            m_teamItem.GetComponent<TeamItem>().Init();
            m_hpBarItem.GetComponent<HPBarItem>().Init();
            m_weaponBox.GetComponent<WeaponItems>().Init();

            GameEntry.Event.Subscribe(KillEvent.EventId, UpdateKillNum);
        }

        private void UpdateKillNum(object sender, GameEventArgs e)
        {
            m_killNum++;
            KillNum.text = m_killNum.ToString();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_miniMap.GetComponent<MiniMap>().UpdatePlayer();

            if (Input.GetKeyDown(KeyCode.G))
            {
                miniMapId = GameEntry.UI.OpenUIForm(UIFormId.MiniMapForm);
            }
            if (Input.GetKeyUp(KeyCode.G) && GameEntry.UI.HasUIForm((int)miniMapId))
            {
                GameEntry.UI.CloseUIForm((int)miniMapId);
            }
        }


    }
}
