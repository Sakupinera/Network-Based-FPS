using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleEntityBasicInfo
{

}

public interface IBattleEntityCompBasicInfo: IBattleEntityBasicInfo
{

}

public class BattleEntityCompBasicInfo : BattleEntityCompBase, IBattleEntityCompBasicInfo
{
    public BattleEntityCompBasicInfo(IBattleEntityCompOwner owner) : base(owner)
    {
    }


}
