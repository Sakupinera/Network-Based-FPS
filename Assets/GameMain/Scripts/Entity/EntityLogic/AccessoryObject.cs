using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 可作为附属实体的实体类
    /// </summary>
    public class AccessoryObject : Entity
    {
        [SerializeField]
        private AccessoryObjectData m_AccessoryObjectData = null;
    }
}