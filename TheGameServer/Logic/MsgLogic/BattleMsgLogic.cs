using GameServer;
using ServerPlayer;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 战斗消息与本地逻辑关联
/// </summary>
public static class BattleMsgLogic
{
    //开始战斗
    public static void MsgStartFight(Player player)
    {
        Console.WriteLine("玩家ID：" + player.playerData.id + "请求开启战斗");
        if (player.playerContext.Status != Status.PREPARE)
        {
            Console.WriteLine("开启战斗失败 玩家状态有误");
            ResponseFightMsg msg = new ResponseFightMsg();
            msg.responseFight = -1;
            player.Send(msg);
            return;
        }
        if (player.playerContext.IsHomeowners != true)
        {
            Console.WriteLine("开启战斗失败 玩家不是房主");
            ResponseFightMsg msg = new ResponseFightMsg();
            msg.responseFight = -2;
            player.Send(msg);
            return;
        }
        Room room = player.playerContext.Room;
        if (room.CanStart() == false)
        {
            Console.WriteLine("开启战斗失败 未达到开战条件");
            ResponseFightMsg msg = new ResponseFightMsg();
            msg.responseFight = -3;
            player.Send(msg);
            return;
        }
        //开始战斗
        Console.WriteLine("开启战斗成功");
        room.StartFight();

    }
}

