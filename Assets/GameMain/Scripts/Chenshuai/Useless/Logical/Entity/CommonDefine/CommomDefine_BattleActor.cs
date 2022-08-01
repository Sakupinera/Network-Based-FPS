using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Õ½¶·×´Ì¬
/// </summary>
public enum BattleActorState
{
    Idle,
    Walk,
    Jump,
    Down
}

public enum MoveDirectionEnum 
{
    /// <summary>
    /// ×ó
    /// </summary>
    LEFT,
    /// <summary>
    /// ÓÒ
    /// </summary>
    RIGHT,
    /// <summary>
    /// Ç°
    /// </summary>
    FRONT,
    /// <summary>
    /// ºó
    /// </summary>
    BACK,
}

public struct OptCmd
{
    public OptCmdType m_optCmdType;
    public MoveDirectionEnum m_moveDirectionEnum;
    public int m_intParam01;
    public int m_intParam02;
    public int m_intParam03;
    public int m_intParam04;
    public int m_intParam05;

    public float m_floatParam01;
    public float m_floatParam02;
    public float m_floatParam03;
    public float m_floatParam04;
    public float m_floatParam05;
}

public enum OptCmdType
{
    Movement,
    Shootting,
}