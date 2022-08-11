using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IBattleActorEntity: IBattleActorHp, IBattleActorBasicInfo,IBattleActorOptCmdInput
{
    /// <summary>
    /// 玩家得到速度
    /// </summary>
    /// <returns></returns>
    int EntitySpeedGet();


}
