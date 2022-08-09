using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 枪械类
    /// </summary>
    public class Gun : Entity
    {
        private const string AttachPoint = "WorldCamera/WeaponPoint";

        [SerializeField]
        private GunData m_GunData = null;

        private float m_NextAttackTime = 0f;

        //当前子弹数量
        public int currentBullects;

        //射击位置
        public Transform shootPoint;

        //开火计时器
        public float fireTimer = 0;
        //装弹计时器
        public float reloadTimer = 0;
        //决定长按开火键多少时间算作连发
        public float cFireMaxTime = 0.1f;

        //子弹发射方向
        public Vector3 shootDirection;

        //武器射击范围
        public float range = 100f;

        public float ReloadRate => m_GunData.ReloadTime;

        //弹道偏移队列
        public Queue<Vector3> excursion = new Queue<Vector3>();

        [SerializeField]
        private Animator m_FirstPersonAnimator;

        public Animator FirstPersonAnimator
        {
            get => m_FirstPersonAnimator;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            shootPoint = transform.Find("Armature/Weapon/ShootPoint");

        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_GunData = userData as GunData;
            if (m_GunData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }
            currentBullects = m_GunData.MagazineSize;
            for (int i = 0; i < m_GunData.Trajectory.Count; i++)
            {
                excursion.Enqueue(m_GunData.Trajectory[i]);
            }

            m_FirstPersonAnimator = GetComponent<Animator>();
            GetComponent<EquipmentAnimation>().AssignAnimations(m_FirstPersonAnimator);

            GameEntry.Entity.AttachEntity(Entity, m_GunData.OwnerId, AttachPoint);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //开火计时器每帧增加
            if (fireTimer < m_GunData.AttackInterval)
            {
                fireTimer += Time.deltaTime;
            }

            //装弹计时器每帧增加
            if (reloadTimer < m_GunData.ReloadTime)
            {
                reloadTimer += Time.deltaTime;
            }
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = new Vector3(0.045f, 0.079f, 0.33f);

            (parentEntity as Player).firstPersonAnimator = m_FirstPersonAnimator;

        }

        //public void TryAttack()
        //{
        //    if (Time.time < m_NextAttackTime)
        //    {
        //        return;
        //    }

        //    m_NextAttackTime = Time.time + m_GunData.AttackInterval;
        //    GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), m_GunData.BulletId, m_GunData.OwnerId, m_GunData.OwnerCamp, m_GunData.Attack, m_GunData.BulletSpeed)
        //    {
        //        Position = CachedTransform.position,
        //    });
        //    GameEntry.Sound.PlaySound(m_GunData.BulletSoundId);
        //}

        public void Fire()
        {

            //控制武器射击速度，两者的差值就是枪械的设计间隔
            if (fireTimer < m_GunData.AttackInterval || currentBullects <= 0 || reloadTimer < m_GunData.ReloadTime)
            {
                m_FirstPersonAnimator.SetTrigger("Fire");
                return;
            }

            // TODO: 射线判定生成弹孔特效
            //RaycastHit hit;
            
            shootDirection = shootPoint.forward + shootPoint.right * Random.Range(0, 0.05f) + shootPoint.up * Random.Range(0, 0.05f);

            //依据发射方向创建子弹预设体
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), m_GunData.BulletId, m_GunData.OwnerId, m_GunData.OwnerCamp, m_GunData.Attack, m_GunData.BulletSpeed)
            {
                Position = shootPoint.position,
                Rotation = shootPoint.rotation
            });

            //创建并且播放枪口特效
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_GunData.MuzzleSparkId)
            {
                Position = shootPoint.position,
                Rotation = shootPoint.rotation
            });

            //播放射击音效
            //fireAudioSource.Play();

            //当前子弹减少
            currentBullects--;

            //重置开火计时器
            fireTimer = 0;

        }

        /// <summary>
        /// 换弹逻辑
        /// </summary>
        /// <param name="key"></param>
        public void ReloadBullet(KeyCode key)
        {
            if (Input.GetKeyDown(key) && currentBullects < m_GunData.MagazineSize && m_GunData.BulletNum > 0)
            {
                //计算出当前子弹数补满一个弹夹需要的的剩余子弹
                int bulletNeed = m_GunData.MagazineSize - currentBullects;

                if (m_GunData.BulletNum >= bulletNeed)
                {
                    m_GunData.BulletNum -= bulletNeed;
                    currentBullects += bulletNeed;
                }
                else if (m_GunData.BulletNum < bulletNeed)
                {
                    currentBullects += m_GunData.BulletNum;
                    m_GunData.BulletNum = 0;
                }
                reloadTimer = 0;

                FirstPersonAnimator.SetTrigger("Reload");
            }
            else
            {
                return;
            }
        }
    }
}