using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using NetworkBasedFPS;

/// <summary>
/// UI血条变化
/// Fire:玩家受伤的时候,玩家初始化的时候
/// Subscribe:UI组件初始化
/// </summary>
public class PlayerOnHPChangedEventArgs : GameEventArgs
{
    /// <summary>
    /// 加载全局配置成功事件编号
    /// </summary>
    public static readonly int EventId = typeof(PlayerOnHPChangedEventArgs).GetHashCode();

    /// <summary>
    /// ID
    /// </summary>
    public override int Id
    {
        get { return EventId; }
    }

    //玩家的当前血量
    int currentHp = 0;
    public int CurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; }
    }

    public PlayerOnHPChangedEventArgs Create(int currentHp)
    {
        
        PlayerOnHPChangedEventArgs playerOnHPChangedEventArgs = new PlayerOnHPChangedEventArgs();
        playerOnHPChangedEventArgs.CurrentHp = currentHp;
        return playerOnHPChangedEventArgs;
    }

    public override void Clear()
    {
        currentHp = 0;
    }
}
