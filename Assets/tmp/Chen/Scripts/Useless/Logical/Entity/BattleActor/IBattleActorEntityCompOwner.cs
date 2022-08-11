using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleActorEntityCompOwner
{
    IBattleActorEnv BattleActorEnvGet();

    IBattleActorCompHp CompBattleActorHpGet();

    IBattleActorCompBasicInfo CompBattleActorBasicInfoGet();
}
