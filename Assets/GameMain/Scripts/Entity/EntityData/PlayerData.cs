using GameFramework.DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    [Serializable]
    public class PlayerData : TargetableObjectData
    {
        [SerializeField]
        private string m_Name = null;

        [SerializeField]
        private List<GunData> m_GunDatas = new List<GunData>();

        [SerializeField]
        private List<MeleeWeaponData> m_MeleeWeaponDatas = new List<MeleeWeaponData>();

        [SerializeField]
        private List<ThrownData> m_ThrownDatas = new List<ThrownData>();

        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private float m_Speed = 0;

        [SerializeField]
        private int m_DeadEffectId = 0;

        [SerializeField]
        private int m_DeadSoundId = 0;

        [SerializeField]
        private int m_FootStepSoundId = 0;

        [SerializeField]
        private int m_HitSoundId = 0;

        public CtrlType CtrlType { get; set; }


        public PlayerData(int entityId, int typeId) : base(entityId, typeId, CampType.Unknown)
        {

            IDataTable<DRCharacter> dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
            DRCharacter drCharacter = dtCharacter.GetDataRow(typeId);
            if (drCharacter == null)
            {
                return;
            }

            m_MaxHP = drCharacter.MaxHP;
            m_Speed = drCharacter.MoveSpeed;
            m_FootStepSoundId = drCharacter.FootStepSoundId;
            HP = m_MaxHP;
            AttachWeaponData(new GunData(GameEntry.Entity.GenerateSerialId(), 30000, Id, Camp));
        }

        /// <summary>
        /// 玩家名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        /// <summary>
        /// 玩家最大生命值
        /// </summary>
        public override int MaxHP
        {
            get => m_MaxHP;
        }

        /// <summary>
        /// 玩家移动速度
        /// </summary>
        public float Speed
        {
            get => m_Speed;
            set => m_Speed = value;
        }

        public List<MeleeWeaponData> GetAllMeleeWeaponDatas()
        {
            return m_MeleeWeaponDatas;
        }

        public void AttachWeaponData(MeleeWeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_MeleeWeaponDatas.Contains(weaponData))
            {
                return;
            }

            m_MeleeWeaponDatas.Add(weaponData);
        }

        public void DetachWeaponData(MeleeWeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            m_MeleeWeaponDatas.Remove(weaponData);
        }

        public List<GunData> GetAllGunDatas()
        {
            return m_GunDatas;
        }

        public void AttachWeaponData(GunData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_GunDatas.Contains(weaponData))
            {
                return;
            }

            m_GunDatas.Add(weaponData);
        }

        public void DetachWeaponData(GunData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            m_GunDatas.Remove(weaponData);
        }

        public List<ThrownData> GetAllThrownDatas()
        {
            return m_ThrownDatas;
        }

        public void AttachThrownData(ThrownData thrownData)
        {
            if (thrownData == null)
            {
                return;
            }

            if (m_ThrownDatas.Contains(thrownData))
            {
                return;
            }

            m_ThrownDatas.Add(thrownData);
        }

        public void DetachThrownData(ThrownData thrownData)
        {
            if (thrownData == null)
            {
                return;
            }

            m_ThrownDatas.Remove(thrownData);
        }
    }
}