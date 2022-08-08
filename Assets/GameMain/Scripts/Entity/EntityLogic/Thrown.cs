using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 可投掷物类
    /// </summary>
    public class Thrown : Entity
    {
        [SerializeField]
        private ThrownData m_ThrownData = null;
    }
}