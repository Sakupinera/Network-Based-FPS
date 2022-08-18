using GameFramework;
using GamePlayer;
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

        public GunData _GunData => m_GunData;

        //  当前弹夹子弹数量
        public int currentMagBullets;

        //  当前子弹总量
        public int currentBulletNum;

        //子弹出生位置
        private Transform m_shootPoint;

        public Transform ShootPoint => m_shootPoint;

        //开火计时器
        public float fireTimer = 0;
        //装弹计时器
        public float reloadTimer = 0;

        //子弹发射方向
        public Vector3 shootDirection;

        //武器动画
        private Animator m_FirstPersonAnimator;

        private Transform m_aimTarget;

        //弹道偏移队列
        public Queue<Vector3> excursion = new Queue<Vector3>();

        public float ReloadRate
        {
            get;
            private set;
        }

        public float AttackInterval => m_GunData.AttackInterval;

        public Animator FirstPersonAnimator
        {
            get => m_FirstPersonAnimator;
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_shootPoint = transform.Find("Armature/Weapon/ShootPoint");
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
            currentMagBullets = m_GunData.MagazineSize;
            currentBulletNum = m_GunData.BulletNum;

            for (int i = 0; i < m_GunData.Trajectory.Count; i++)
            {
                excursion.Enqueue(m_GunData.Trajectory[i]);
            }
            ReloadRate = m_GunData.ReloadTime;

            m_FirstPersonAnimator = GetComponent<Animator>();
            GetComponent<EquipmentAnimation>().AssignAnimations(m_FirstPersonAnimator);

            // 挂载自身到父物体身上
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
            if (reloadTimer < ReloadRate)
            {
                reloadTimer += Time.deltaTime;
            }
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = m_GunData.AttachLocalPosition;

            gameObject.SetActive(false);
        }

        public void Fire()
        {
            if (currentMagBullets <= 0)
            {
                ReloadBullet();
                return;
            }
            //控制武器射击速度，两者的差值就是枪械的设计间隔
            if (fireTimer < m_GunData.AttackInterval || reloadTimer < ReloadRate)
            {
                return;
            }
            m_FirstPersonAnimator.SetTrigger("Fire");

            GameEntry.Sound.PlaySound(m_GunData.BulletSoundId);

            //射线判定子弹目标点
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            Vector3 target;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point; //  记录碰撞的目标点
            }
            else
            {
                target = Camera.main.transform.forward * 800;
            }
            m_shootPoint.LookAt(target);
            shootDirection = m_shootPoint.forward + m_shootPoint.right * Random.Range(-0.01f, 0.01f) + m_shootPoint.up * Random.Range(-0.01f, 0.01f);
            m_shootPoint.transform.forward = shootDirection;

            //依据发射方向创建子弹预设体
            GameEntry.Entity.ShowBullet(new BulletData(GameEntry.Entity.GenerateSerialId(), m_GunData.BulletId, m_GunData.OwnerId, m_GunData.OwnerCamp, m_GunData.Attack, m_GunData.BulletSpeed)
            {
                Position = m_shootPoint.position,
                Rotation = m_shootPoint.rotation
            });

            SendBulletMsg(m_shootPoint.position, m_shootPoint.rotation);

            //创建并且播放枪口特效
            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_GunData.MuzzleSparkId)
            {
                Position = m_shootPoint.position,
                Rotation = m_shootPoint.rotation
            });

            //播放射击音效
            //fireAudioSource.Play();

            //当前子弹减少
            currentMagBullets--;
            print(currentMagBullets);
            GameEntry.Event.Fire(this, WeaponOnBulletChangedEventArgs.Create(currentMagBullets, currentBulletNum, _GunData.TypeId));

            //重置开火计时器
            fireTimer = 0;

        }

        //发射子弹协议
        public void SendBulletMsg(Vector3 p, Quaternion r)
        {
            ShootMsg msg = new ShootMsg();
            msg.id = GameEntry.Net.ID;
            GamePlayer.Bullet bullet = new GamePlayer.Bullet();
            bullet.posX = p.x;
            bullet.posY = p.y;
            bullet.posZ = p.z;
            bullet.rotX = r.x;
            bullet.rotY = r.y;
            bullet.rotZ = r.z;
            bullet.rotW = r.w;

            msg.bullet = bullet;
            GameEntry.Net.Send(msg);
        }

        /// <summary>
        /// 换弹逻辑
        /// </summary>
        /// <param name="key"></param>
        public void ReloadBullet()
        {
            if (currentMagBullets < m_GunData.MagazineSize && m_GunData.BulletNum > 0)
            {

                (GameEntry.Entity.GetParentEntity(Id).Logic as Player).SendWeaponInfo(-1, true);
                // 播放换弹动画
                if (currentMagBullets > 0)
                {
                    m_FirstPersonAnimator.SetTrigger("Reload");
                    ReloadRate = m_GunData.ReloadTime;
                }
                else
                {
                    m_FirstPersonAnimator.SetTrigger("Empty Reload");
                    ReloadRate = m_GunData.EmptyReloadTime;
                }
                Player p = (Player)GameEntry.Entity.GetParentEntity(Id).Logic;
                p.ThridPersonAnimator.SetTrigger("Reload");

                //计算出当前子弹数补满一个弹夹需要的的剩余子弹
                int bulletNeed = m_GunData.MagazineSize - currentMagBullets;

                if (m_GunData.BulletNum >= bulletNeed)
                {
                    currentBulletNum -= bulletNeed;
                    currentMagBullets += bulletNeed;
                }
                else if (m_GunData.BulletNum < bulletNeed)
                {
                    currentMagBullets += currentBulletNum;
                    currentBulletNum = 0;
                }
                reloadTimer = 0;

                GameEntry.Event.Fire(this, WeaponOnBulletChangedEventArgs.Create(currentMagBullets, currentBulletNum, _GunData.TypeId));
            }
            else
            {
                return;
            }
        }

        //  重置武器的状态
        public void ResetGunData()
        {
            currentMagBullets = m_GunData.MagazineSize;
            currentBulletNum = m_GunData.BulletNum;

            excursion.Clear();
            for (int i = 0; i < m_GunData.Trajectory.Count; i++)
            {
                excursion.Enqueue(m_GunData.Trajectory[i]);
            }
            ReloadRate = m_GunData.ReloadTime;
        }
    }
}