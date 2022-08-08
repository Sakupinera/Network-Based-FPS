using System;
using UnityEngine;

namespace NetworkBasedFPS
{
    [Serializable]
    public class EffectData : EntityData
    {
        [SerializeField]
        private float m_KeepTime = 0f;

        public EffectData(int entityId, int typeId)
            : base(entityId, typeId)
        {
            m_KeepTime = 2f;
        }

        public float KeepTime
        {
            get
            {
                return m_KeepTime;
            }
        }
    }
}
