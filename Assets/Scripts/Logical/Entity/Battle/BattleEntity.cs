using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : IBattleEntity, IBattleEntityCompOwner
{
    protected IBattleEntityEnv m_env;

    public List<BattleEntityCompBase> m_battleEntityCompList = new List<BattleEntityCompBase>();

    public BattleEntityCompBasicInfo m_battleEntityComBasicInfo;

    public BattleEntityCompBattleActorEnv m_battleEntityCompBattleActorEnv;

    public BattleEntityCompBattleActorManager m_battleEntityCompBattleActorManager;

    public BattleEntityCompOptCmd m_battleEntityCompOptCmd;


    public BattleEntity(IBattleEntityEnv env)
    {
        m_env = env;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_battleEntityComBasicInfo = new BattleEntityCompBasicInfo(this);
        m_battleEntityComBasicInfo.Initialize();
        m_battleEntityCompList.Add(m_battleEntityComBasicInfo);

        m_battleEntityCompBattleActorEnv = new BattleEntityCompBattleActorEnv(this);
        m_battleEntityCompBattleActorEnv.Initialize();
        m_battleEntityCompList.Add(m_battleEntityCompBattleActorEnv);

        m_battleEntityCompBattleActorManager = new BattleEntityCompBattleActorManager(this);
        m_battleEntityCompBattleActorManager.Initialize();
        m_battleEntityCompList.Add(m_battleEntityCompBattleActorManager);

        m_battleEntityCompOptCmd = new BattleEntityCompOptCmd(this);
        m_battleEntityCompOptCmd.Initialize();
        m_battleEntityCompList.Add(m_battleEntityCompOptCmd);

    }

    #region 组件对内接口实现
    public IBattleEntityCompBattleActorEnv BattleEntityBattleActorEnvGet()
    {
        throw new System.NotImplementedException();
    }

    public IBattleEntityCompBattleActorManager BattleEntityBattleActorManagerGet()
    {
        throw new System.NotImplementedException();
    }

    public IBattleEntityCompBasicInfo BattleEntityCompBasicInfoGet()
    {
        throw new System.NotImplementedException();
    }

    public IBattleEntityCompOptCmd BattleEntityCompOptCmdGet()
    {
        return m_battleEntityCompOptCmd;
    }
    #endregion


}
