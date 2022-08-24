using GameFramework.Event;
using GamePlayer;
using GameServer;
using GameSystem;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using System.Collections.Generic;

namespace NetworkBasedFPS
{
    public class MiniMapForm : UGuiForm
    {
        private TeamGame teamGame;
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

        private RectTransform rect;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            terrain = GameObject.Find("对象006");

            rect = miniMap.GetComponent<Image>().rectTransform;
            //获取当前的战斗流程
            procedureBattle = GameEntry.Procedure.CurrentProcedure as ProcedureBattle;
            if (procedureBattle.CurrentGame.GameMode == GameMode.Team)
            {
                teamGame = (procedureBattle.CurrentGame as TeamGame);
            }

            //miniMap = this.transform.Find("img_miniMap").gameObject;
            //for (int i = 0; i < teamGame.list.Count; i++)
            //{
            //    //找到本地玩家
            //    if (player.GetPlayerData.CtrlType == CtrlType.player)
            //    {
            //        m_player = player;
            //        //将本地玩家设置 的图标设置为红色
            //        GameObject img = Instantiate(miniPlayer, miniMap.transform);
            //        img.GetComponent<Image>().color = Color.red;
            //        images.Add(player, img.GetComponent<Image>());
            //    }
            //}

        }

        /// <summary>
        /// 每帧进行更新玩家在小地图上的位置
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            UpdatePlayer();
        }

        /// <summary>
        /// 更新玩家在小地图上的位置 逻辑
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        private void UpdatePlayer()
        {
            foreach (Player player in teamGame.list.Values)
            {
                if (player.GetPlayerData.CtrlType == CtrlType.player && m_player == null)
                {
                    m_player = player;
                    //将本地玩家设置 的图标设置为红色
                    GameObject img = Instantiate(miniPlayer, miniMap.transform);
                    img.GetComponent<Image>().color = Color.red;
                    images.Add(player, img.GetComponent<Image>());
                }
                else if (m_player != null)
                {
                    break;
                }
            }
            foreach (Player player in teamGame.list.Values)
            {


                ////计算玩家在小地图上的位置
                //float x = (player.transform.position.x / (terrain.GetComponent<Collider>().bounds.size.x / 2f)) * (miniMap.GetComponent<Image>().rectTransform.rect.width / 2);
                //float y = (player.transform.position.z / (terrain.GetComponent<Collider>().bounds.size.z / 2f)) * (miniMap.GetComponent<Image>().rectTransform.rect.height / 2);

                float realWidth = terrain.GetComponent<Collider>().bounds.size.x;
                float realheigh = terrain.GetComponent<Collider>().bounds.size.z;

                float realX = player.transform.position.x - terrain.GetComponent<Collider>().bounds.min.x;
                float realY = player.transform.position.z - terrain.GetComponent<Collider>().bounds.min.z;

                float pivotX = realX / realWidth;
                float pivotY = realY / realheigh;

                float itemX = pivotX * rect.rect.width - rect.rect.width / 2;
                float itemY = pivotY * rect.rect.height - rect.rect.height / 2;

                //找到本地玩家
                if (player.GetPlayerData.CtrlType == CtrlType.player)
                {
                    images[player].rectTransform.anchoredPosition = new Vector2(itemX, itemY);
                    images[player].rectTransform.eulerAngles = new Vector3(0, 0, -player.transform.eulerAngles.y);

                }

                //判断是否为相同阵营
                if (m_player != null)
                {
                    //判断是否为相同阵营
                    if (teamGame.IsSameCamp(m_player.gameObject, player.gameObject) && player.GetPlayerData.CtrlType != CtrlType.player)
                    {
                        //字典中包含
                        if (images.ContainsKey(player))
                        {
                            images[player].rectTransform.anchoredPosition = new Vector2(itemX, itemY);
                            images[player].rectTransform.eulerAngles = new Vector3(0, 0, -player.transform.eulerAngles.y);
                        }
                        //字典中不包含
                        else
                        {
                            GameObject img = Instantiate(miniPlayer, miniMap.transform);
                            img.GetComponent<Image>().color = Color.white;
                            img.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(itemX, itemY);
                            img.GetComponent<Image>().rectTransform.eulerAngles = new Vector3(0, 0, -player.transform.eulerAngles.y);
                            images.Add(player, img.GetComponent<Image>());
                        }

                    }
                }

            }


        }


    }
}

