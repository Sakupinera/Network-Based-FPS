using GameServer;
using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 管理房间
/// </summary>
public class RoomMgr
{
    //单例
    private static RoomMgr instance;

    public static RoomMgr GetInstance()
    {
        if (instance == null)
            instance = new RoomMgr();
        return instance;
    }

    //房间列表
    public List<Room> roomsList = new List<Room>();

    //创建房间
    public void CreateRoom(Player player)
    {
        Room room = new Room();
        lock (roomsList)
        {
            roomsList.Add(room);
            room.AddPlayer(player);
        }
    }

    //玩家离开
    public void LeaveRoom(Player player)
    {
        PlayerContext playerContext = player.playerContext;
        //若该玩家为空闲状态则直接返回
        if (playerContext.Status == ServerPlayer.Status.FREE)
            return;

        Room room = playerContext.Room;

        lock (roomsList)
        {
            room.DelPlayer(player.playerData.id);
            //房间玩家为0删除房间
            if (room.roomPlayersList.Count == 0)
                roomsList.Remove(room);
        }
    }

    //房间列表
    public RoomListMsg GetRoomList()
    {
        RoomListMsg msg = new RoomListMsg();
        List<RoomData> roomDatas = new List<RoomData>();

        //每个房间信息
        foreach (Room room in roomsList)
        {
            RoomData roomData = new RoomData();
            roomData.roomStatus = room.status;
            roomData.roomPlayersNum = room.roomPlayersList.Count;
            roomDatas.Add(roomData);
        }
        msg.roomList = roomDatas;
        return msg;
    }
}

