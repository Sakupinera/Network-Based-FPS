using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    public class EscForm : UGuiForm
    {
        private ProcedureBattle m_ProcedureBattle = null;

        [SerializeField]
        private Button m_ReturnButton = null;

        [SerializeField]
        private Button m_SettingButton = null;

        [SerializeField]
        private Button m_ExitButton = null;

        public void OnReturnButtonClick()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Close();
        }

        public void OnSettingButtonClick()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }

        public void OnExitButtonClick()
        {
            //  TODO: 退出游戏

        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_ReturnButton.onClick.AddListener(OnReturnButtonClick);
            m_SettingButton.onClick.AddListener(OnSettingButtonClick);
            m_ExitButton.onClick.AddListener(OnExitButtonClick);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureBattle = (ProcedureBattle)userData;
            if(m_ProcedureBattle == null)
            {
                Log.Warning("ProcedureBattle is invalid when open EscForm.");
                return;
            }
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}

