using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleEntityBattleActorEnv
{

}

public interface IBattleEntityCompBattleActorEnv:IBattleActorEnv 
{

}


public class BattleEntityCompBattleActorEnv : BattleEntityCompBase, IBattleEntityCompBattleActorEnv
{
    public BattleEntityCompBattleActorEnv(IBattleEntityCompOwner owner) : base(owner)
    {
    }

    public void OptCmdPush(OptCmd optCmd)
    {
        
    }
}
