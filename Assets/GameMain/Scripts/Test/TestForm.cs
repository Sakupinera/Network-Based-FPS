using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS {
    public class TestForm : UGuiForm
    {
        //[SerializeField]
        //private GameObject m_QuitButton = null;

        private ProcedureMainMenu m_ProcedureMainMenu = null;

        public void OnStartButtonClick()
        {
            Debug.Log("按钮被点击！");
            m_ProcedureMainMenu.StartGame();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_ProcedureMainMenu = (ProcedureMainMenu)userData;
            if (m_ProcedureMainMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            //m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            m_ProcedureMainMenu = null;

            base.OnClose(isShutdown, userData);
        }
    }
}