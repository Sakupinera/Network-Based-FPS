using GameServer;
using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 让收到房间信息请求消息的Handler与本地逻辑关联
/// </summary>
public static class RoomMsgLogic
{
    //获取房间列表
    public static void MsgGetRoomList(Player player)
    {
        try
        {
            Console.WriteLine(player.playerData.id + " 请求房间列表");
            player.Send(RoomMgr.GetInstance().GetRoomList());
        }
        catch (Exception e)
        {
            Console.WriteLine("获取房间列表有误" + e.Message);
        }
    }

    //创建房间
    public static void MsgCreateRoom(Player player)
    {
        try
        {
            //若玩家不是空闲状体则不能创建房间（正在房间内准备或者正在战斗）
            if (player.playerContext.Status != ServerPlayer.Status.FREE)
            {
                Console.WriteLine("创建房间失败 " + player.playerData.id);
                //player.Send(protocol);
                return;
            }
            RoomMgr.GetInstance().CreateRoom(player);
            Console.WriteLine("创建房间成功 " + player.playerData.id);
        }
        catch (Exception e)
        {
            Console.WriteLine("创建房间有误" + e.Message);
        }
    }

    //加入房间
    public static void MsgEnterRoom(Player player, int index)
    {
        Console.WriteLine("进入房间ID：" + player.playerData.id);
        try
        {
            //显示信息
            Console.WriteLine("收到进入房间消息 ID：" + player.playerData.id + " 房间号：" + index);
            //判断房间是否存在
            if (index < 0 || index >= RoomMgr.GetInstance().roomsList.Count)
            {
                Console.WriteLine("进入房间失败 房间编号有误" + player.playerData.id);
                return;
            }
            Room room = RoomMgr.GetInstance().roomsList[index];
            //判断房间状态
            if (room.status != E_ROOM_STATUS.PREPARE)
            {
                Console.WriteLine("进入房间失败 房间状态有误 " + player.playerData.id);
                return;
            }
            //添加玩家
            if (room.AddPlayer(player))
            {
                //添加玩家成功后刷新房间信息
                room.Broadcast(room.GetRoomInfoMsg());
            }
            else
            {
                Console.WriteLine("进入房间失败 已满员" + player.playerData.id);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("进入房间有误" + e.Message);
        }
    }

    //获取房间信息
    public static void MsgGetRoomInfo(Player player)
    {
        try
        {
            //若玩家不是在房间中的准备状态则不发送房间信息
            if (player.playerContext.Status != ServerPlayer.Status.PREPARE)
            {
                Console.WriteLine("获取房间信息失败 玩家状态有误 " + player.playerData.id + " " + player.playerContext.Status);
                return;
            }
            Room room = player.playerContext.Room;
            player.Send(room.GetRoomInfoMsg());

        }
        catch (Exception e)
        {
            Console.WriteLine("获取房间信息有误" + e.Message);
        }

    }

    //离开房间
    public static void MsgLeaveRoom(Player player)
    {
        try
        {
            //若玩家状态不是在房间中的准备状态则不能离开房间
            if (player.playerContext.Status != ServerPlayer.Status.PREPARE)
            {
                Console.WriteLine("离开房间失败 玩家状态有误 " + player.playerData.id);
                //player.Send(protocol);
                return;
            }
            //处理
            Room room = player.playerContext.Room;
            RoomMgr.GetInstance().LeaveRoom(player);
            //广播
            if (room != null)
                room.Broadcast(room.GetRoomInfoMsg());
        }
        catch (Exception e)
        {
            Console.WriteLine("离开房间有误" + e.Message);
        }
    }

    //切换阵营
    public static void ChangeCamp(Player player, GamePlayer.E_PLAYER_CAMP camp)
    {
        try
        {
            if (player.playerContext.Status != Status.PREPARE)
            {
                Console.WriteLine("切换阵营失败 玩家状态有误 " + player.playerData.id);
                return;
            }
            //切换
            Room room = player.playerContext.Room;
            room.ChangeCamp(player, camp);
            //广播
            if (room != null)
                room.Broadcast(room.GetRoomInfoMsg());
        }
        catch (Exception e)
        {
            Console.WriteLine("切换阵营有误" + e.Message);
        }
    }

}

