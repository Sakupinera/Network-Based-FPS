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
        private Image m_BackgroundImage;

        public void SetText(string name, bool isOner, E_PLAYER_CAMP camp)
        {
            m_NameText.text = name;
            if (isOner)
                m_NameText.color = Color.yellow;
            //显示阵营
            if (camp == E_PLAYER_CAMP.A)
            {
                m_BackgroundImage.color = Color.red;
            }
            else if (camp == E_PLAYER_CAMP.B)
            {
                m_BackgroundImage.color = Color.blue;
            }
            else
            {
                m_BackgroundImage.color = Color.white;
                print("other");
            }
        }

        public void DestroyMySelf()
        {
            Destroy(gameObject);
        }
    }

}
