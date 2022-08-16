using GameFramework.Event;
using NetworkBasedFPS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnShowEventArgs : GameEventArgs
{
    //事件编号
    public static readonly int EventId = typeof(PlayerOnShowEventArgs).GetHashCode();

    //ID
    public override int Id
    {
        get { return EventId; }
    }

    //private Player player;

    //事件参数
    //public Player Player
    //{
    //    get { return player; }
    //    private set { player = value; }
    //}
    //实体ID
    int playerEntryID = 0;
    public int PlayerEntryID
    {
        get { return playerEntryID; }
        private set { playerEntryID = value; }
    }


    public static PlayerOnShowEventArgs Create(int playerEntryID)
    {
        PlayerOnShowEventArgs playerOnShowEventArgs = new PlayerOnShowEventArgs();
        //playerOnShowEventArgs.player = player;
        playerOnShowEventArgs.PlayerEntryID = playerEntryID;
        return playerOnShowEventArgs;
    }


    public override void Clear()
    {
        PlayerEntryID = 0;
    }
}
