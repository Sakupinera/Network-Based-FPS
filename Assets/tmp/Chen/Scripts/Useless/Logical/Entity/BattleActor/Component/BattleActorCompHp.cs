using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对外接口
/// </summary>
public interface IBattleActorHp
{
    float CurrentHpGet();

    float MaxHpGet();
} 

/// <summary>
/// 对内接口
/// </summary>
public interface IBattleActorCompHp: IBattleActorHp
{

}


public class BattleActorCompHp : BattleActorCompBase, IBattleActorCompHp
{
    public BattleActorCompHp(IBattleActorEntityCompOwner owner) : base(owner)
    {
    }

    public float CurrentHpGet()
    {
        throw new System.NotImplementedException();
    }

    public float MaxHpGet()
    {
        throw new System.NotImplementedException();
    }
}
