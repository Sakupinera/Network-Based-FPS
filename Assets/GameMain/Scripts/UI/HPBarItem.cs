using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class HPBarItem : UGuiForm
    {
        [SerializeField]
        protected GameObject HPBar;
        private Slider HPBarSlider;

        public void Init()
        {
            HPBarSlider = HPBar.GetComponent<Slider>();

            GameEntry.Event.Subscribe(PlayerOnHPChangedEventArgs.EventId, ChangeHpValue);
        }

        /// <summary>
        /// 设置UI界面的血条数值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeHpValue(object sender,GameEventArgs e)
        {
            PlayerOnHPChangedEventArgs playerOnHPChangedEventArgs = (PlayerOnHPChangedEventArgs)e;

            HPBarSlider.value = playerOnHPChangedEventArgs.CurrentHp/100f;
        }


    }
}
