using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对外接口
/// </summary>
public interface IBattleActorPosition
{
    float HorizontalGet();

    float VerticalGet();
}

/// <summary>
/// 对内接口
/// </summary>
public interface IBattleActorCompPostion :IBattleActorPosition
{

}

/// <summary>
/// 战斗人物位置组件
/// </summary>
public class BattleActorCompPosition : BattleActorCompBase, IBattleActorCompPostion
{
    private float m_horizontal;
    private float m_vertical;

    public float Horizontal { get { return m_horizontal; } set { m_horizontal = value; } }
    public float Vertical { get { return m_vertical; } set { m_vertical = value; } }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="owner">组件所属的单位</param>
    /// <param name="horizontal"></param>
    /// <param name="vertical">实体的初始位置</param>
    public BattleActorCompPosition(IBattleActorEntityCompOwner owner, float horizontal, float vertical) : base(owner)
    {
        m_horizontal = horizontal;
        m_vertical = vertical;
    }

    #region IBattleActorPosition接口实现
    public float HorizontalGet()
    {
        return Horizontal;
    }

    public float VerticalGet()
    {
        return Vertical;
    }
    #endregion
}
