using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleEntityOptCmd
{

}

public interface IBattleEntityCompOptCmd: IBattleEntityOptCmd
{
    void OptCmdPush(OptCmd optCmd);
}

public class BattleEntityCompOptCmd : BattleEntityCompBase, IBattleEntityCompOptCmd
{
    protected Queue<OptCmd> m_optCmdQueue = new Queue<OptCmd>();

    public BattleEntityCompOptCmd(IBattleEntityCompOwner owner) : base(owner)
    {
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);

        var optCmd = m_optCmdQueue.Dequeue();
        OptCmdExec(optCmd);
    }

    protected void OptCmdExec(OptCmd optCmd)
    {
        switch (optCmd.m_optCmdType)
        {
            case OptCmdType.Movement:
                m_owner.BattleEntityBattleActorManagerGet();
                break;
            case OptCmdType.Shootting:
                break;    
        }
    }

    public void OptCmdPush(OptCmd optCmd)
    {
        m_optCmdQueue.Enqueue(optCmd);
    }
}
