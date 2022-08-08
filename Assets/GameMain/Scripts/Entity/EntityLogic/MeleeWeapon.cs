using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 近战武器类
    /// </summary>
    public class MeleeWeapon : Entity
    {
        [SerializeField]
        private MeleeWeaponData m_MeleeWeaponData = null;
    }
}