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
        private GameObject HPBar;

        [SerializeField]
        private TMP_Text HPBarText;

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
        public void ChangeHpValue(object sender, GameEventArgs e)
        {
            PlayerOnHPChangedEventArgs playerOnHPChangedEventArgs = (PlayerOnHPChangedEventArgs)e;

            HPBarSlider.value = playerOnHPChangedEventArgs.CurrentHp / 100f;
            HPBarText.text = playerOnHPChangedEventArgs.CurrentHp.ToString();
        }

        public void OnClose()
        {
            GameEntry.Event.Unsubscribe(PlayerOnHPChangedEventArgs.EventId, ChangeHpValue);

        }
    }
}
