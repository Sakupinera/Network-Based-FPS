using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;
using GameFramework;
using NetworkBasedFPS;

/// <summary>
/// Fire:玩家武器开火的时候调用
/// </summary>
public class WeaponOnBulletChangedEventArgs : GameEventArgs
{
    /// <summary>
    /// 加载全局配置成功事件编号
    /// </summary>
    public static readonly int EventId = typeof(WeaponOnBulletChangedEventArgs).GetHashCode();

    /// <summary>
    /// ID
    /// </summary>
    public override int Id
    {
        get { return EventId; }
    }

    int currentBullet = 0;
    public int CurrentBullet
    {
        get { return currentBullet; }
        set { currentBullet = value; }
    }

    int restBullet = 0;
    public int RestBullet
    {
        get { return restBullet; }
        set { restBullet = value; }
    }

    int weaponTypeID = 0;
    public int WeaponTypeID
    {
        get { return weaponTypeID; }
        set { weaponTypeID = value; }
    }

    public static WeaponOnBulletChangedEventArgs Create(int currentBullet, int RestBullet, int weaponTypeId)
    {
        WeaponOnBulletChangedEventArgs weaponOnBulletChangedEventArgs = new WeaponOnBulletChangedEventArgs();
        weaponOnBulletChangedEventArgs.currentBullet = currentBullet;
        weaponOnBulletChangedEventArgs.RestBullet = RestBullet;
        weaponOnBulletChangedEventArgs.weaponTypeID = weaponTypeId;
        return weaponOnBulletChangedEventArgs;
    }


    public override void Clear()
    {
        this.currentBullet = 0;
        this.RestBullet = 0;
        this.weaponTypeID = 0;
    }

}
