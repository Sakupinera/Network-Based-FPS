using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对外接口
/// </summary>
public interface IBattleActorMovement
{

}

/// <summary>
/// 组件对内接口
/// </summary>
public interface IBattleActorCompMovement: IBattleActorMovement
{

}

public class BattleActorCompMovement : BattleActorCompBase, IBattleActorCompMovement
{
    public BattleActorCompMovement(IBattleActorEntityCompOwner owner) : base(owner)
    {
    }


}
