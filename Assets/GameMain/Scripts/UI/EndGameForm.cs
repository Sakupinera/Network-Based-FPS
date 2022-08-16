using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{

    public class EndGameForm : UGuiForm
    {
        [SerializeField]
        private Image m_WinerImage_1;

        [SerializeField]
        private Image m_WinerImage_2;

        [SerializeField]
        private Button m_BackToRoomButton;

        private int m_Winer;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_Winer = (int)userData;
            if (m_Winer == 1)
            {
                m_WinerImage_1.enabled = true; m_WinerImage_2.enabled = false;
            }
            else
            {
                m_WinerImage_1.enabled = false; m_WinerImage_2.enabled = true;
            }
            m_BackToRoomButton.onClick.AddListener(OnBackToRoomButtonClick);
        }

        private void OnBackToRoomButtonClick()
        {
            //(ProcedureBattle)GameEntry.Procedure.CurrentProcedure
            Close();
        }
    }
}
