using GameFramework.Event;
using GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class PopForm : UGuiForm
    {
        [SerializeField]
        private Text m_PopText;

        [SerializeField]
        private Button m_PopButton;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_PopText.text = userData.ToString();
            print(m_PopText.text);

            m_PopButton.onClick.AddListener(ClosePop);
        }

        public void ClosePop()
        {
            Close();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {

            base.OnClose(isShutdown, userData);
        }

    }

}
