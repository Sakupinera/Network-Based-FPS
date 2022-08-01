using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleEntityCompBase
{
    protected IBattleEntityCompOwner m_owner;

    protected BattleEntityCompBase(IBattleEntityCompOwner owner)
    {
        m_owner = owner;
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Initialize()
    {

    }

    public virtual void Tick(float deltaTime)
    {

    }

    /// <summary>
    /// п╤ть
    /// </summary>
    public virtual void UnInitialize()
    {

    }
}
