using GameFramework.DataTable;
using System;
using UnityEngine;

namespace NetworkBasedFPS
{
    [Serializable]
    public class MeleeWeaponData : AccessoryObjectData
    {
        [SerializeField]
        int m_MeleeSoundId = 0;

        [SerializeField]
        int m_Attack = 0;

        [SerializeField]
        int m_AttackInterval = 0;

        [SerializeField]
        float m_AttackRadius = 0;

        public MeleeWeaponData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {

        }
    }
}
