using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GamePlayer;
using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class TeamGame : GameBase
    {
        public override GameMode GameMode => GameMode.Team;

        //战场中所有玩家
        //玩家实体ID 对应 玩家实体
        public Dictionary<int, Player> list = new Dictionary<int, Player>();
        //NetID 对应 玩家实体ID
        public Dictionary<int, int> Netlist = new Dictionary<int, int>();


        public override void Initialize()
        {
            base.Initialize();
            GameEntry.Event.Subscribe(PlayerOnShowEventArgs.EventId, SetPlayerList);
            GameEntry.Event.Subscribe(MsgEventArgs<GetListMsg>.EventId, StartBattle);
            GameEntry.Event.Subscribe(MsgEventArgs<MoveMsg>.EventId, UpdatePlayerMoveInfo);
            GameEntry.Event.Subscribe(MsgEventArgs<StatusMsg>.EventId, UpdatePlayerStatusInfo);
            GameEntry.Event.Subscribe(MsgEventArgs<ShootMsg>.EventId, UpdateShootInfo);
            GameEntry.Event.Subscribe(MsgEventArgs<WeaponMsg>.EventId, UpdateWeapon);
            GameEntry.Event.Subscribe(MsgEventArgs<DamageMsg>.EventId, UpdateDamage);
            GameEntry.Event.Subscribe(MsgEventArgs<EndFightMsg>.EventId, EndFight);
            Debug.Log("开始团队模式");
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.Event.Unsubscribe(PlayerOnShowEventArgs.EventId, SetPlayerList);
            GameEntry.Event.Unsubscribe(MsgEventArgs<GetListMsg>.EventId, StartBattle);
            GameEntry.Event.Unsubscribe(MsgEventArgs<MoveMsg>.EventId, UpdatePlayerMoveInfo);
            GameEntry.Event.Unsubscribe(MsgEventArgs<StatusMsg>.EventId, UpdatePlayerStatusInfo);
            GameEntry.Event.Unsubscribe(MsgEventArgs<ShootMsg>.EventId, UpdateShootInfo);
            GameEntry.Event.Unsubscribe(MsgEventArgs<WeaponMsg>.EventId, UpdateWeapon);
            GameEntry.Event.Unsubscribe(MsgEventArgs<DamageMsg>.EventId, UpdateDamage);
            GameEntry.Event.Unsubscribe(MsgEventArgs<EndFightMsg>.EventId, EndFight);
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("输出list");
                foreach (int i in Netlist.Keys)
                {
                    Debug.Log(i + " " + Netlist[i]);
                }

                foreach (int i in list.Keys)
                {
                    Debug.Log(i + " " + list[i].GetPlayerData.Id);
                }
            }
        }

        //获取阵营
        public CampType GetCamp(GameObject playerObj)
        {
            foreach (Player player in list.Values)
            {
                if (player.gameObject == playerObj)
                    return player.GetPlayerData.Camp;
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
            Netlist.Clear();
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
            int swopID_A = 0;
            int swopID_B = 0;
            //每一个玩家
            for (int i = 0; i < num; i++)
            {
                CampType camp;
                int swopID = 0;
                if (msg.list[i].playerCamp == E_PLAYER_CAMP.A)
                {
                    camp = CampType.BlueCamp;
                    swopID = ++swopID_A;
                }
                else if (msg.list[i].playerCamp == E_PLAYER_CAMP.B)
                {
                    camp = CampType.RedCamp;
                    swopID = ++swopID_B;
                }
                else
                {
                    Debug.Log("阵容有误");
                    return;
                }
                GeneratePlayer(msg.list[i].id, msg.list[i].name, camp, swopID);
            }
        }

        //生成玩家
        public void GeneratePlayer(int netId, string name, CampType team, int swopID, int modelId = 11000)
        {
            if (team == CampType.BlueCamp)
                modelId = 11000;
            if (team == CampType.RedCamp)
                modelId = 11001;
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
            int entityID = GameEntry.Entity.GenerateSerialId();
            //绑定NetID 和 实体ID
            Netlist.Add(netId, entityID);
            Debug.Log("NetID: " + netId + " 实体ID:" + entityID);


            //产生玩家
            GameEntry.Entity.ShowPlayer(new PlayerData(entityID, modelId)
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
            Player player = GameEntry.Entity.GetEntity(id).Logic as Player;
            Debug.Log(player.name);

            list.Add(id, player);
            Debug.Log("实体ID：" + id + " 实体ID：" + player.GetPlayerData.Id);

            //玩家处理
            if (id == Netlist[GameEntry.Net.ID])
            {
                Debug.Log("玩家 " + id + " " + list[id].Name + " 为玩家操控");
                list[id].GetPlayerData.CtrlType = CtrlType.player;
                list[id].InitPLayerCtrl();
            }
            else
            {
                Debug.Log("玩家 " + id + " " + list[id].Name + " 为Net操控");
                list[id].GetPlayerData.CtrlType = CtrlType.net;
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
            Player player = list[Netlist[pos.id]];
            player.NetForecastInfo(new Vector3(pos.posX, pos.posY, pos.posZ), new Vector3(pos.rotX, pos.rotY, pos.rotZ), new Vector3(pos.tPosX, pos.tPosY, pos.tPosZ));
        }


        //更新玩家状态
        private void UpdatePlayerStatusInfo(object sender, GameEventArgs e)
        {
            MsgEventArgs<StatusMsg> msgEventArgs = (MsgEventArgs<StatusMsg>)e;
            StatusMsg msg = msgEventArgs.Msg;
            if (msg.playerStatus.id == GameEntry.Net.ID)
            {
                return;
            }
            Player player = list[Netlist[msg.playerStatus.id]];
            player.playerStatus = msg.playerStatus.playerStatus;
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
        }

        //更新武器
        private void UpdateWeapon(object sender, GameEventArgs e)
        {
            MsgEventArgs<WeaponMsg> msgEventArgs = (MsgEventArgs<WeaponMsg>)e;
            WeaponMsg msg = msgEventArgs.Msg;
            if (msg.id == GameEntry.Net.ID)
            {
                return;
            }
            Player player = list[Netlist[msg.id]];
            player.NetWeapon(msg.weaponID, msg.isReload);
        }

        //伤害同步
        private void UpdateDamage(object sender, GameEventArgs e)
        {
            MsgEventArgs<DamageMsg> msgEventArgs = (MsgEventArgs<DamageMsg>)e;
            DamageMsg msg = msgEventArgs.Msg;
            if (msg.isKilled == true)
            {
                GameEntry.Event.Fire(this, PlayerScoreChangedEventArgs.Create(list[msg.injured].GetPlayerData.Camp));
            }
            Player player = list[msg.injured];
            if (msg.id == GameEntry.Net.ID)
            {
                if (list[msg.injured].GetPlayerData.CtrlType == CtrlType.player && msg.damage != 0)
                {
                    Debug.LogError("自杀");
                    player.isSuicide = true;
                }
                return;
            }
            player.GetPlayerData.HP -= msg.damage;

            if (msg.injured == Netlist[GameEntry.Net.ID])
            {
                GameEntry.Event.Fire(this, PlayerOnHPChangedEventArgs.Create(player.GetPlayerData.HP));
            }
        }


        //结束游戏
        private void EndFight(object sender, GameEventArgs e)
        {
            MsgEventArgs<EndFightMsg> msgEventArgs = (MsgEventArgs<EndFightMsg>)e;
            int winer = msgEventArgs.Msg.fightResult;
            GameEntry.UI.OpenUIForm(UIFormId.EndGameForm, winer);
            GameOver = true;
        }
    }
}
