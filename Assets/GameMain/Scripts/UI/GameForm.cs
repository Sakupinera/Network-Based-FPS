using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
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


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_miniMap.GetComponent<MiniMap>().Init();
            m_teamItem.GetComponent<TeamItem>().Init();
            m_hpBarItem.GetComponent<HPBarItem>().Init();
            m_weaponBox.GetComponent<WeaponItems>().Init();
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_miniMap.GetComponent<MiniMap>().UpdatePlayer();
        }
    }
}
