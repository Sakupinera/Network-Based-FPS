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
        private TMP_Text teamScoreA;

        [SerializeField]
        private TMP_Text teamScoreB;

        private int tAScore = 0;
        private int tBScore = 0;

        public void Init()
        {
            UpdateScore();

            GameEntry.Event.Subscribe(PlayerScoreChangedEventArgs.EventId, ChangeScore);
        }

        /// <summary>
        /// 计算分数
        /// </summary>
        protected void ChangeScore(object sender, GameEventArgs e)
        {
            print("加分");
            PlayerScoreChangedEventArgs playerScoreChangedEventArgs = (PlayerScoreChangedEventArgs)e;
            if (playerScoreChangedEventArgs.CampType == CampType.BlueCamp)
            {
                tAScore++;
            }
            if (playerScoreChangedEventArgs.CampType == CampType.RedCamp)
            {
                tBScore++;
            }

            UpdateScore();
        }


        public void UpdateScore()
        {
            teamScoreA.text = tAScore.ToString();
            teamScoreB.text = tBScore.ToString();
        }

        public void OnClose()
        {
            GameEntry.Event.Unsubscribe(PlayerScoreChangedEventArgs.EventId, ChangeScore);

        }
    }
}
