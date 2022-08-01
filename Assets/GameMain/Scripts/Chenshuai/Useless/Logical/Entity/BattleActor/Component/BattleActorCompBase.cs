using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleActorCompBase
{
    //帮助获取其他组件的一些对内接口的句柄
    protected IBattleActorEntityCompOwner m_owner;

    protected BattleActorCompBase(IBattleActorEntityCompOwner owner)
    {
        m_owner = owner;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialize()
    {

    }

    /// <summary>
    /// update函数，用来做一帧一帧逻辑
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void Tick(float deltaTime)
    {

    }

    /// <summary>
    /// 卸载
    /// </summary>
    public virtual void UnInitialize()
    {

    }
}
