using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework.Event;
using GamePlayer;
using GameServer;
using GameSystem;
using System;
using UnityGameFramework.Runtime;


namespace NetworkBasedFPS
{
    public class MiniMap : UGuiForm
    {
        private TeamGame teamGame = new TeamGame();
        ProcedureBattle procedureBattle;

        //玩家的图片组件以及物体
        [SerializeField]
        private GameObject miniPlayer;

        //地形物件
        [SerializeField]
        private GameObject terrain;

        //小地图
        [SerializeField]
        private GameObject miniMap;

        [SerializeField]
        private Player m_player;
        private Dictionary<Player, Image> images = new Dictionary<Player, Image>();


        public void Init()
        {
            //获取当前的战斗流程
            procedureBattle = GameEntry.Procedure.CurrentProcedure as ProcedureBattle;
            if (procedureBattle.CurrentGame.GameMode == GameMode.Team)
            {
                teamGame = (procedureBattle.CurrentGame as TeamGame);
            }

            //miniMap = this.transform.Find("img_miniMap").gameObject;
            for (int i = 0; i < teamGame.list.Count; i++)
            {
                //找到本地玩家
                if (teamGame.list[i].GetPlayerData.CtrlType == CtrlType.player)
                {
                    m_player = teamGame.list[i];
                    //将本地玩家设置 的图标设置为红色
                    GameObject img = Instantiate(miniPlayer, miniMap.transform);
                    img.GetComponent<Image>().color = Color.red;
                    images.Add(teamGame.list[i], img.GetComponent<Image>());
                }
            }
        }

        public void UpdatePlayer()
        {
            for (int i = 0; i < teamGame.list.Count; i++)
            {
                //计算玩家在小地图上的位置
                float x = (teamGame.list[i].transform.position.x / (terrain.GetComponent<Collider>().bounds.size.x / 2f)) * (miniMap.GetComponent<Image>().rectTransform.rect.width / 2);
                float y = (teamGame.list[i].transform.position.z / (terrain.GetComponent<Collider>().bounds.size.z / 2f)) * (miniMap.GetComponent<Image>().rectTransform.rect.height / 2);

                //找到本地玩家
                if (teamGame.list[i].GetPlayerData.CtrlType == CtrlType.player)
                {
                    images[teamGame.list[i]].rectTransform.anchoredPosition = new Vector2(x, y);
                    images[teamGame.list[i]].rectTransform.eulerAngles = new Vector3(0, 0, -teamGame.list[i].transform.eulerAngles.y);

                    float realX = (teamGame.list[i].transform.position.x + terrain.GetComponent<Collider>().bounds.size.x / 2f) / terrain.GetComponent<Collider>().bounds.size.x;
                    float realY = (teamGame.list[i].transform.position.z + terrain.GetComponent<Collider>().bounds.size.z / 2f) / terrain.GetComponent<Collider>().bounds.size.z;

                    miniMap.GetComponent<Image>().rectTransform.pivot = new Vector2(realX, realY);
                    miniMap.GetComponent<Image>().rectTransform.localPosition = Vector2.zero;
                    miniMap.GetComponent<Image>().rectTransform.eulerAngles = new Vector3(0, 0, teamGame.list[i].transform.eulerAngles.y);
                    

                }

                //判断是否为相同阵营
                if (teamGame.IsSameCamp(m_player.gameObject, teamGame.list[i].gameObject))
                {
                    //字典中包含
                    if (images.ContainsKey(teamGame.list[i]))
                    {
                        images[teamGame.list[i]].rectTransform.anchoredPosition = new Vector2(x, y);
                        images[teamGame.list[i]].rectTransform.eulerAngles = new Vector3(0, 0, -teamGame.list[i].transform.eulerAngles.y);
                    }
                    //字典中不包含
                    else
                    {
                        GameObject img = Instantiate(miniPlayer, miniMap.transform);
                        img.GetComponent<Image>().color = Color.white;
                        img.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(x, y);
                        img.GetComponent<Image>().rectTransform.eulerAngles = new Vector3(0, 0, -teamGame.list[i].transform.eulerAngles.y);
                        images.Add(teamGame.list[i], img.GetComponent<Image>());
                    }

                }

            }


        }
    }
}
