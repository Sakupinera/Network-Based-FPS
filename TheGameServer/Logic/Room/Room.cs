using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GameServer;
using GamePlayer;

/// <summary>
/// 房间系统
/// </summary>
public class Room
{
    //房间分为准备阶段和正在游戏阶段
    //public enum Status
    //{
    //    PREPARE = 1,
    //    FIGHT = 2,
    //}

    //初始为准备状态
    public E_ROOM_STATUS status = E_ROOM_STATUS.PREPARE;
    //房间最大玩家数量
    public int maxPlayersNum = 10;
    //用字典维护房间玩家
    public Dictionary<int, Player> roomPlayersList = new Dictionary<int, Player>();
    //房主
    private int oner;

    //添加玩家
    public bool AddPlayer(Player player)
    {
        lock (roomPlayersList)
        {
            //超过房间最大人数则进入失败
            if (roomPlayersList.Count >= maxPlayersNum)
                return false;
            PlayerContext playerContext = player.playerContext;
            playerContext.Room = this;
            //playerContext.Team = SwichTeam();
            //player.playerData.playerCamp = playerContext.Team;
            ChangeCamp(player, SwichTeam());
            playerContext.Status = ServerPlayer.Status.PREPARE;


            int id = player.playerData.id;
            if (roomPlayersList.Count == 0)
            {
                playerContext.IsHomeowners = true;
                oner = id;
            }
            roomPlayersList.Add(id, player);
            //Console.WriteLine("ID：" + id + "进入房间\n房主为" + oner + "\n现在房间玩家有：");
            //foreach (int p in roomPlayersList.Keys)
            //{
            //    Console.WriteLine("ID:" + p);
            //}
        }
        return true;
    }

    //删除玩家
    public void DelPlayer(int id)
    {
        lock (roomPlayersList)
        {
            if (!roomPlayersList.ContainsKey(id))
                return;
            bool isOwner = roomPlayersList[id].playerContext.IsHomeowners;
            roomPlayersList[id].playerContext.Status = ServerPlayer.Status.FREE;
            roomPlayersList.Remove(id);
            //Console.WriteLine("ID：" + id + " 离开房间 ");
            //如果退出的是房主则更换房主
            if (isOwner)
                UpdateOwner();
        }
    }


    //分配队伍
    public E_PLAYER_CAMP SwichTeam()
    {
        //统计阵营人数
        int campANum = 0;
        int campBNum = 0;
        foreach (Player player in roomPlayersList.Values)
        {
            if (player.playerContext.Team == E_PLAYER_CAMP.A) campANum++;
            if (player.playerContext.Team == E_PLAYER_CAMP.B) campBNum++;
        }
        if (campANum <= campBNum)
            return E_PLAYER_CAMP.A;
        else
            return E_PLAYER_CAMP.B;
    }

    //更换房主
    public void UpdateOwner()
    {
        lock (roomPlayersList)
        {
            if (roomPlayersList.Count <= 0)
                return;

            foreach (Player player in roomPlayersList.Values)
            {
                player.playerContext.IsHomeowners = false;
            }
            //将第一个玩家设为房主
            Player p = roomPlayersList.Values.First();
            p.playerContext.IsHomeowners = true;
            oner = p.playerData.id;
        }
    }

    //对房间内玩家进行消息广播
    public void Broadcast(BaseMsg baseMsg)
    {
        foreach (Player player in roomPlayersList.Values)
        {
            player.Send(baseMsg);
        }
    }

    //房间信息
    public RoomInfoMsg GetRoomInfoMsg()
    {
        RoomInfoMsg roomInfoMsg = new RoomInfoMsg();
        roomInfoMsg.Oner = oner;
        List<PlayerData> list = new List<PlayerData>();
        foreach (Player player in roomPlayersList.Values)
        {
            list.Add(player.playerData);
        }
        roomInfoMsg.roomPlayersList = list;
        return roomInfoMsg;
    }


    //开始战斗
    public void StartFight()
    {
        //更改房间状态
        status = E_ROOM_STATUS.FIGHT;
        lock (roomPlayersList)
        {
            ResponseFightMsg msg = new ResponseFightMsg();
            msg.responseFight = 1;
            //修改玩家状态
            foreach (Player player in roomPlayersList.Values)
            {
                player.playerContext.Status = Status.FIGHT;
            }
            //广播玩家信息PlayerData
            Broadcast(msg);
        }
    }

    //是否能开始战斗
    public bool CanStart()
    {
        //若房间已经在战斗状态
        if (status == E_ROOM_STATUS.FIGHT)
            return false;

        //统计阵营人数
        int campANum = 0;
        int campBNum = 0;
        foreach (Player player in roomPlayersList.Values)
        {
            if (player.playerContext.Team == E_PLAYER_CAMP.A) campANum++;
            if (player.playerContext.Team == E_PLAYER_CAMP.B) campBNum++;
        }
        //若有一方人数为0则不能开始战局
        if (campANum == 0 || campBNum == 0)
            return false;
        return true;
    }

    //更换阵营
    public void ChangeCamp(Player player, E_PLAYER_CAMP camp)
    {
        if (player.playerContext.Team != camp)
        {
            player.playerContext.Team = camp;
            player.playerData.playerCamp = player.playerContext.Team;
        }
    }
}

