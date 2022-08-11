using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对外接口
/// </summary>
public interface IBattleActorBasicInfo
{

}

/// <summary>
/// 对内接口
/// </summary>
public interface IBattleActorCompBasicInfo:IBattleActorBasicInfo
{

}

public class BattleActorComBasicInfo : BattleActorCompBase,IBattleActorCompBasicInfo
{


    public BattleActorComBasicInfo(IBattleActorEntityCompOwner owner) : base(owner)
    {
    }


}
