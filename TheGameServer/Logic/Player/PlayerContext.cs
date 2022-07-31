using GamePlayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerPlayer
{
    /// <summary>
    /// 玩家3个状态
    /// （进入房间前）空闲
    /// 准备中
    /// 战斗中
    /// </summary>
    public enum Status
    {
        FREE,
        PREPARE,
        FIGHT,
    }
    /// <summary>
    /// 记录玩家当前状态
    /// </summary>
    public class PlayerContext
    {
        public PlayerContext()
        {
            Status = Status.FREE;
            IsHomeowners = false;
            Team = E_PLAYER_CAMP.A;
            Status = Status.FREE;
        }
        //状态
        public Status Status { get; set; }

        //对应的房间状态
        public Room Room { get; set; }
        public E_PLAYER_CAMP Team { get; set; }
        public bool IsHomeowners { get; set; }

        //对应战斗状态
        public int HP { get; set; }

    }
}
