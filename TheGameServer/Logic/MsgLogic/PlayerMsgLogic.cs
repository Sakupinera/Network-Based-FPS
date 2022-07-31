using GamePlayer;
using GameScene;
using GameServer;
using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 让收到玩家消息的Handler与本地逻辑关联
/// </summary>
public static class PlayerMsgLogic
{
    /// <summary>
    /// 将指定id的clientsocket与player绑定，并传入playerdata
    /// </summary>
    /// <param name="id">玩家id</param>
    /// <param name="playerData">玩家数据</param>
    public static void Login(int id, PlayerData playerData)
    {
        if (ServerSocket.GetInstance().GetClient(id) != null && ServerSocket.GetInstance().GetClient(id).player == null)
        {
            Console.WriteLine("ID：" + id + " 注册");
            Player player = new Player(playerData, ServerSocket.GetInstance().GetClient(id));
            ServerSocket.GetInstance().GetClient(id).player = player;
            Scene.GetInstance().AddPlayer(id);
        }
    }

    /// <summary>
    /// 解除绑定并断开连接
    /// </summary>
    /// <param name="id">玩家id</param>
    public static void Logout(int id)
    {
        if (ServerSocket.GetInstance().GetClient(id) != null)
        {
            //退出房间
            Player player = ServerSocket.GetInstance().GetClient(id).player;
            if (player.playerContext.Status != ServerPlayer.Status.FREE)
            {
                Room room = player.playerContext.Room;
                RoomMgr.GetInstance().LeaveRoom(player);
                if (room != null)
                    room.Broadcast(room.GetRoomInfoMsg());
            }
            //断开连接
            Console.WriteLine("ID：" + id + " 断开");
            ServerSocket.GetInstance().GetClient(id).Close();
            Scene.GetInstance().DelPlayer(id);
        }
    }

    /// <summary>
    /// 更新玩家位置（坐标、朝向）
    /// </summary>
    /// <param name="playerPos">解析的玩家位置信息</param>
    public static void UpdatePlayerPos(PlayerPos playerPos)
    {
        Scene.GetInstance().UpdatePlayerPosInfo(playerPos);
    }

    /// <summary>
    /// 更新状态信息
    /// </summary>
    /// <param name="playerStatus"></param>
    public static void UpdatePlayerStatus(PlayerStatus playerStatus)
    {
        Scene.GetInstance().UpdatePlayerStatusInfo(playerStatus);
    }

    //广播场景中玩家位置信息
    public static void SendPlayersPos()
    {
        //直接广播消息
        ServerSocket.GetInstance().Broadcast(Scene.GetInstance().PkgPlayersPos());
    }

    //广播场景中玩家状态信息
    public static void SendPlayersStatus()
    {
        //直接广播消息
        ServerSocket.GetInstance().Broadcast(Scene.GetInstance().PkgPlayersStatus());
    }
}

