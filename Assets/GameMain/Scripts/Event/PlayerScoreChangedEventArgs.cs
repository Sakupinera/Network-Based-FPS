using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using NetworkBasedFPS;

/// <summary>
/// 玩家得分事件，
/// Fire:每有个玩家触发死亡时
/// SubsCribe:计分板进行订阅
/// 
/// </summary>
public class PlayerScoreChangedEventArgs : GameEventArgs
{

    /// <summary>
    /// 加载全局配置成功事件编号
    /// </summary>
    public static readonly int EventId = typeof(PlayerScoreChangedEventArgs).GetHashCode();

    /// <summary>
    /// ID
    /// </summary>
    public override int Id
    {
        get { return EventId; }
    }


    /// <summary>
    /// 击杀者的阵营
    /// </summary>
    CampType campType;
    public CampType CampType
    {
        get { return campType; }
        private set { campType = value; }
    }

    public static PlayerScoreChangedEventArgs Create(CampType campType)
    {

        PlayerScoreChangedEventArgs playerScoreChangedEventArgs = new PlayerScoreChangedEventArgs();
        playerScoreChangedEventArgs.CampType = campType;
        return playerScoreChangedEventArgs;
    }

    public override void Clear()
    {
        this.campType = CampType.Unknown;
    }
}
