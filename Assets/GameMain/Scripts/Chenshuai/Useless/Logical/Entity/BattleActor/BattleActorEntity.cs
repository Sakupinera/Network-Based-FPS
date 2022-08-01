using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleActorEntity : IBattleActorEntity, IBattleActorEntityCompOwner
{

    protected List<BattleActorCompBase> m_battleActorCompList = new List<BattleActorCompBase>();

    protected BattleActorComBasicInfo m_comBattleActorComBasicInfo;

    protected BattleActorCompHp m_comBattleActorHp;

    protected BattleActorCompOptCmdInput m_comBattleActorOptCmdInput;

    protected IBattleActorEnv m_battleActorEnv;


    /// <summary>
    /// ctor 构造函数
    /// </summary>
    public BattleActorEntity(IBattleActorEnv env)
    {
        m_battleActorEnv = env;
    }

    #region IBattleActorEnv 接口实现
    public IBattleActorEnv BattleActorEnvGet()
    {
        return m_battleActorEnv;
    }
    #endregion

    #region IBattleActorCompBasicInfo接口实现
    public IBattleActorCompBasicInfo CompBattleActorBasicInfoGet()
    {
        return m_comBattleActorComBasicInfo;
    }
    #endregion

    #region IBattleActorCompHp接口实现
    public IBattleActorCompHp CompBattleActorHpGet()
    {
        return m_comBattleActorHp;
    }
    #endregion

    #region IBattleActorHp接口实现
    public float CurrentHpGet()
    {
        return m_comBattleActorHp.CurrentHpGet();
    }

    public float MaxHpGet()
    {
        return m_comBattleActorHp.MaxHpGet();
    }
    #endregion

    #region IBattleActorBasicInfo接口实现
    public int EntitySpeedGet()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region IBattleActorOptCmdInput接口实现
    public void OptCmdInput(OptCmd optCmd)
    {
        m_comBattleActorOptCmdInput.OptCmdInput(optCmd);
    }
    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_comBattleActorHp = new BattleActorCompHp(this);
        m_comBattleActorHp.Initialize();
        m_battleActorCompList.Add(m_comBattleActorHp);

        m_comBattleActorComBasicInfo = new BattleActorComBasicInfo(this);
        m_comBattleActorComBasicInfo.Initialize();
        m_battleActorCompList.Add(m_comBattleActorComBasicInfo);

        m_comBattleActorOptCmdInput = new BattleActorCompOptCmdInput(this);
        m_comBattleActorOptCmdInput.Initialize();
        m_battleActorCompList.Add(m_comBattleActorOptCmdInput);
    }

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick(float deltaTime)
    {

    }

    /// <summary>
    /// 卸载
    /// </summary>
    public virtual void UnInitialize()
    {

    }

}
