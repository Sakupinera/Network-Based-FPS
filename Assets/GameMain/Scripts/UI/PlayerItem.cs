using GamePlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class PlayerItem : UGuiForm
    {
        [SerializeField]
        private Text m_NameText;

        [SerializeField]
        private Image m_BackgroundImage_1;

        [SerializeField]
        private Image m_BackgroundImage_2;

        public void SetText(string name, bool isOner, E_PLAYER_CAMP camp)
        {
            m_NameText.text = name;
            //显示阵营
            if (camp == E_PLAYER_CAMP.A)
            {

                m_BackgroundImage_1.enabled = true;
                m_BackgroundImage_2.enabled = false;
                m_NameText.color = Color.white;
            }
            else if (camp == E_PLAYER_CAMP.B)
            {
                m_BackgroundImage_1.enabled = false;
                m_BackgroundImage_2.enabled = true;
                m_NameText.color = Color.black;
            }
            else
            {
                print("other");
            }
            if (isOner)
                m_NameText.color = Color.yellow;
        }

        public void DestroyMySelf()
        {
            Destroy(gameObject);
        }
    }

}
