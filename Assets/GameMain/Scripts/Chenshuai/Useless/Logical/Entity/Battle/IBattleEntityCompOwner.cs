using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗实例组件所有者接口，提供战斗实例组件获取方法
/// </summary>
public interface IBattleEntityCompOwner
{
    IBattleEntityCompBasicInfo BattleEntityCompBasicInfoGet();

    IBattleEntityCompBattleActorEnv BattleEntityBattleActorEnvGet();

    IBattleEntityCompBattleActorManager BattleEntityBattleActorManagerGet();

    IBattleEntityCompOptCmd BattleEntityCompOptCmdGet();

}
