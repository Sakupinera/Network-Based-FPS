using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    [Serializable]
    public class GunData : AccessoryObjectData
    {
        [SerializeField]
        private int m_Attack = 0;

        [SerializeField]
        private float m_AttackInterval = 0;

        [SerializeField]
        private float m_AttackRange = 0;

        [SerializeField]
        private Vector3 m_AttachLocalPosition = new Vector3();

        [SerializeField]
        private int m_MagazineSize = 0;

        [SerializeField]
        private int m_BulletNum = 0;

        [SerializeField]
        private float m_ReloadTime = 0;

        [SerializeField]
        private float m_EmptyReloadTime = 0;

        [SerializeField]
        private int m_BulletId = 0;

        [SerializeField]
        private float m_BulletSpeed = 0f;

        [SerializeField]
        private int m_BulletSoundId = 0;

        [SerializeField]
        private int m_MuzzleSparkId = 0;

        [SerializeField]
        private int m_BulletHoleId = 0;

        [SerializeField]
        private List<Vector3> m_Trajectory = null;

        public GunData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {
            IDataTable<DRGun> dtGun = GameEntry.DataTable.GetDataTable<DRGun>();
            DRGun drGun = dtGun.GetDataRow(TypeId);
            if (drGun == null)
            {
                return;
            }

            m_Attack = drGun.Attack;
            m_AttackInterval = drGun.AttackInterval;
            m_AttackRange = drGun.AttackRange;
            m_AttachLocalPosition = drGun.AttachPosition;
            m_MagazineSize = drGun.MagazineSize;
            m_BulletNum = drGun.BulletMaxSize;
            m_ReloadTime = drGun.ReloadTime;
            m_EmptyReloadTime = drGun.EmptyReloadTime;
            m_BulletId = drGun.BulletId;
            m_BulletSpeed = drGun.BulletSpeed;
            m_BulletSoundId = drGun.FireSoundId;
            m_MuzzleSparkId = drGun.MuzzleSparkId;
            m_BulletHoleId = drGun.BulletHoleId;
            m_Trajectory = drGun.Trajectory;
        }

        /// <summary>
        /// 枪械单发伤害
        /// </summary>
        public int Attack
        {
            get => m_Attack;
        }

        /// <summary>
        /// 射击间隔
        /// </summary>
        public float AttackInterval
        {
            get => m_AttackInterval;
        }

        /// <summary>
        /// 射击范围
        /// </summary>
        public float AttackRange
        {
            get => m_AttackRange;
        }

        public Vector3 AttachLocalPosition
        {
            get => m_AttachLocalPosition;
        }

        /// <summary>
        /// 一个弹夹的子弹数量
        /// </summary>
        public int MagazineSize
        {
            get => m_MagazineSize;
        }

        /// <summary>
        /// 子弹总量
        /// </summary>
        public int BulletNum
        {
            get => m_BulletNum;
        }

        /// <summary>
        /// 换弹时间
        /// </summary>
        public float ReloadTime
        {
            get => m_ReloadTime;
        }

        public float EmptyReloadTime
        {
            get => m_EmptyReloadTime;
        }

        /// <summary>
        /// 子弹编号。
        /// </summary>
        public int BulletId
        {
            get
            {
                return m_BulletId;
            }
        }

        /// <summary>
        /// 子弹速度。
        /// </summary>
        public float BulletSpeed
        {
            get
            {
                return m_BulletSpeed;
            }
        }

        /// <summary>
        /// 子弹声音编号。
        /// </summary>
        public int BulletSoundId
        {
            get
            {
                return m_BulletSoundId;
            }
        }

        /// <summary>
        /// 枪口火花特效编号
        /// </summary>
        public int MuzzleSparkId
        {
            get => m_MuzzleSparkId;
        }

        /// <summary>
        /// 子弹弹孔特效Id
        /// </summary>
        public int BulletHoleId
        {
            get => m_BulletHoleId;
        }

        public List<Vector3> Trajectory
        {
            get => m_Trajectory;
        }
    }
}
