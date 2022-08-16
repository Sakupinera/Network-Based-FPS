using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class TeamGame : GameBase
    {
        public override GameMode GameMode => GameMode.Team;

        //战场中所有玩家
        public Dictionary<int, Player> list = new Dictionary<int, Player>();

        public override void Initialize()
        {
            base.Initialize();
            GameEntry.Event.Subscribe(MsgEventArgs<GetListMsg>.EventId, StartBattle);
            GameEntry.Event.Subscribe(PlayerOnShowEventArgs.EventId, SetPlayerList);
            GameEntry.Event.Subscribe(MsgEventArgs<MoveMsg>.EventId, UpdatePlayerMoveInfo);
            GameEntry.Event.Subscribe(MsgEventArgs<ShootMsg>.EventId, UpdateShootInfo);
            Debug.Log("开始团队模式");
        }


        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
        }

        //获取阵营
        public CampType GetCamp(GameObject playerObj)
        {
            foreach (Player player in list.Values)
            {
                if (player.gameObject == playerObj)
                    return player._PlayerData.Camp;
            }
            return 0;
        }

        //是否同一阵营
        public bool IsSameCamp(GameObject tank1, GameObject tank2)
        {
            return GetCamp(tank1) == GetCamp(tank2);
        }

        //清理场景
        public void ClearBattle()
        {
            list.Clear();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {

            }
        }

        //开始战斗
        private void StartBattle(object sender, GameEventArgs e)
        {
            Debug.Log("开始战斗");
            MsgEventArgs<GetListMsg> msgEventArgs = (MsgEventArgs<GetListMsg>)e;
            GetListMsg msg = msgEventArgs.Msg;
            //玩家数量
            int num = msg.list.Count;
            Debug.Log("一共有玩家 " + num + " 个");
            //清理场景
            ClearBattle();
            //每一个玩家
            int swopID_A = 0;
            int swopID_B = 0;
            for (int i = 0; i < num; i++)
            {
                CampType camp;
                if (msg.list[i].playerCamp == E_PLAYER_CAMP.A)
                {
                    camp = CampType.BlueCamp;
                    swopID_A++;
                }
                else if (msg.list[i].playerCamp == E_PLAYER_CAMP.B)
                {
                    camp = CampType.RedCamp;
                    swopID_B++;
                }
                else
                {
                    Debug.Log("阵容有误");
                    return;
                }
                GeneratePlayer(msg.list[i].id, msg.list[i].name, camp, swopID_A);
            }
        }

        //生成玩家
        public void GeneratePlayer(int id, string name, CampType team, int swopID, int modelId = 11001)
        {
            Debug.Log("生成玩家 " + name);
            //获取出生点
            Transform sp = GameObject.Find("SwopPoints").transform;
            Transform swopTrans;
            if (team == CampType.BlueCamp)
            {
                Transform teamSwop = sp.GetChild(0);
                swopTrans = teamSwop.GetChild(swopID - 1);
            }
            else
            {
                Transform teamSwop = sp.GetChild(1);
                swopTrans = teamSwop.GetChild(swopID - 1);
            }
            if (swopTrans == null)
            {
                Debug.LogError("出生点错误！");
                return;
            }
            //产生玩家
            Debug.LogWarning(swopTrans.position);
            GameEntry.Entity.ShowPlayer(new PlayerData(id, 11001)
            {
                Name = name,
                Position = swopTrans.position,
                Camp = team,
            });

        }

        //列表处理
        private void SetPlayerList(object sender, GameEventArgs e)
        {
            PlayerOnShowEventArgs args = (PlayerOnShowEventArgs)e;
            int id = args.PlayerEntryID;
            Debug.Log("实体ID " + id);
            if (GameEntry.Entity.HasEntity(id))
            {
                Debug.Log("有实体");
            }
            else
            {
                Debug.Log("没有实体");
            }
            Player player = GameEntry.Entity.GetEntity(id).Logic as Player;
            Debug.Log(player.name);
            list.Add(id, player);
            //玩家处理
            if (id == GameEntry.Net.ID)
            {
                list[id]._PlayerData.CtrlType = CtrlType.player;
                Debug.Log("玩家 " + list[id].Name + " 为玩家操控");
            }
            else
            {
                Debug.Log("玩家 " + list[id].Name + " 为Net操控");
                list[id]._PlayerData.CtrlType = CtrlType.net;
                list[id].InitNetCtrl();  //初始化网络同步
            }
        }

        //更新玩家位置
        private void UpdatePlayerMoveInfo(object sender, GameEventArgs e)
        {
            MsgEventArgs<MoveMsg> msgEventArgs = (MsgEventArgs<MoveMsg>)e;
            PlayerPos pos = msgEventArgs.Msg.playerPos;
            if (pos.id == GameEntry.Net.ID)
            {
                return;
            }
            Player player = list[pos.id];
            player.NetForecastInfo(new Vector3(pos.posX, pos.posY, pos.posZ), new Vector3(pos.rotX, pos.rotY, pos.rotZ));
        }

        //更新玩家状态
        private void UpdatePlayerStatusInfo(object sender, GameEventArgs e)
        {

        }

        //子弹发射
        private void UpdateShootInfo(object sender, GameEventArgs e)
        {
            MsgEventArgs<ShootMsg> msgEventArgs = (MsgEventArgs<ShootMsg>)e;
            ShootMsg msg = msgEventArgs.Msg;
            if (msg.id == GameEntry.Net.ID)
            {
                return;
            }
            Debug.Log("生成子弹");
            GamePlayer.Bullet bullet = msg.bullet;
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), 50000, 0, CampType.Unknown, 0, 60)
            {
                Position = new Vector3(bullet.posX, bullet.posY, bullet.posZ),
                Rotation = new Quaternion(bullet.rotX, bullet.rotY, bullet.rotZ, bullet.rotW)
            });

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70000)
            {
                Position = new Vector3(bullet.posX, bullet.posY, bullet.posZ),
                Rotation = new Quaternion(bullet.rotX, bullet.rotY, bullet.rotZ, bullet.rotW)
            });

        }
    }
}
