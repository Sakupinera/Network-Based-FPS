using GamePlayer;
using GameServer;
using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameScene
{
    public class Scene
    {
        //目前只有一个场景，使用单例调试更加方便，后续再实现多房间多场景游戏
        private static Scene instance;
        public static Scene GetInstance()
        {
            if (instance == null)
                instance = new Scene();
            return instance;
        }

        //场景中的角色列表
        public List<ScenePlayer> scenePlayersList = new List<ScenePlayer>();

        //获取该场景中指定id的玩家
        private ScenePlayer GetPlayer(int id)
        {
            for (int i = 0; i < scenePlayersList.Count; i++)
            {
                if (scenePlayersList[i].id == id)
                    return scenePlayersList[i];
            }
            return null;
        }

        //在该场景中添加玩家
        public void AddPlayer(int id)
        {
            lock (scenePlayersList)
            {
                ScenePlayer p = new ScenePlayer();
                p.playerPos = new PlayerPos();
                p.playerStatus = new PlayerStatus();
                p.id = id;
                scenePlayersList.Add(p);
            }
        }

        //移除该场景中的玩家
        public void DelPlayer(int id)
        {
            lock (scenePlayersList)
            {
                ScenePlayer p = GetPlayer(id);
                if (p != null)
                    scenePlayersList.Remove(p);
            }
        }

        //对该场景中玩家的发送全员信息
        //public void SendPlayers(Player player)
        //{
        //    UpdatePosInfoMsg updatePosInfoMsg = new UpdatePosInfoMsg();
        //    UpdateStatusInfoMsg updateStatusInfoMsg = new UpdateStatusInfoMsg();
        //    for (int i = 0; i < scenePlayersList.Count; i++)
        //    {
        //        ScenePlayer p = scenePlayersList[i];
        //        updatePosInfoMsg.posInfoList.Add(p.playerPos);
        //        updateStatusInfoMsg.statusInfoList.Add(p.playerStatus);
        //    }
        //    player.Send(updatePosInfoMsg);
        //    player.Send(updateStatusInfoMsg);
        //}

        //封装所有玩家位置信息
        public UpdatePosInfoMsg PkgPlayersPos()
        {
            UpdatePosInfoMsg updatePosInfoMsg = new UpdatePosInfoMsg();
            updatePosInfoMsg.posInfoList = new List<PlayerPos>();
            for (int i = 0; i < scenePlayersList.Count; i++)
            {
                ScenePlayer p = scenePlayersList[i];
                Console.WriteLine("ID:" + p.id + " 位置:" + p.playerPos.posX + " " + p.playerPos.posY + " " + p.playerPos.posZ);
                updatePosInfoMsg.posInfoList.Add(p.playerPos);
            }
            return updatePosInfoMsg;
        }


        //封装所有玩家状态信息
        public UpdateStatusInfoMsg PkgPlayersStatus()
        {
            UpdateStatusInfoMsg updateStatusInfoMsg = new UpdateStatusInfoMsg();
            updateStatusInfoMsg.statusInfoList = new List<PlayerStatus>();
            for (int i = 0; i < scenePlayersList.Count; i++)
            {
                ScenePlayer p = scenePlayersList[i];
                updateStatusInfoMsg.statusInfoList.Add(p.playerStatus);
            }
            return updateStatusInfoMsg;
        }


        //更新场景中的玩家位置信息
        public void UpdatePlayerPosInfo(/*int id, */PlayerPos playerPos)
        {
            ScenePlayer p = GetPlayer(playerPos.id);
            if (p == null)
                return;
            p.playerPos = playerPos;
        }

        //更新场景中玩家状态信息
        public void UpdatePlayerStatusInfo(/*int id, */PlayerStatus playerStatus)
        {
            ScenePlayer p = GetPlayer(playerStatus.id);
            if (p == null)
                return;
            p.playerStatus = playerStatus;
        }

    }
}
