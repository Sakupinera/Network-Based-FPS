using GameFramework.DataTable;
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
            IDataTable<DREffect> dtGun = GameEntry.DataTable.GetDataTable<DREffect>();
            DREffect drGun = dtGun.GetDataRow(TypeId);
            if (drGun == null)
            {
                return;
            }

            m_KeepTime = drGun.KeepTime;
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
