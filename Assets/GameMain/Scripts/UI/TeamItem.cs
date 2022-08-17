using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NetworkBasedFPS
{
    public class TeamItem : UGuiForm
    {
        [SerializeField]
        protected GameObject teamScoreA;
        protected TextMeshPro tMPA;

        [SerializeField]
        protected GameObject teamScoreB;
        protected TextMeshPro tMPB;

        protected int tAScore;
        protected int tBScore;

        public void Init()
        {
            //获取组件
            tMPA = teamScoreA.GetComponent<TextMeshPro>();
            tMPB = teamScoreB.GetComponent<TextMeshPro>();

            GameEntry.Event.Subscribe(PlayerScoreChangedEventArgs.EventId, ChangeScore);
        }

        /// <summary>
        /// 计算分数
        /// </summary>
        protected void ChangeScore(object sender,GameEventArgs e)
        {
            PlayerScoreChangedEventArgs playerScoreChangedEventArgs = (PlayerScoreChangedEventArgs)e;
            if(playerScoreChangedEventArgs.CampType == CampType.BlueCamp)
            {
                tAScore++;
            }
            if(playerScoreChangedEventArgs.CampType == CampType.RedCamp)
            {
                tBScore++;
            }

            UpdateScore();
        }
        

        public void UpdateScore()
        {
            tMPA.text = tAScore.ToString();
            tMPB.text = tBScore.ToString();
        }
    }
}
