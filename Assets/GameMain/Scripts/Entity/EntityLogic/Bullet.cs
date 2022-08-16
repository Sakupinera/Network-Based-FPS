using UnityEngine;
using UnityGameFramework.Runtime;
using GamePlayer;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 子弹类。
    /// </summary>
    public class Bullet : Entity
    {

        [SerializeField]
        private BulletData m_BulletData = null;

        private float m_ElapseSeconds = 0f;

        public ImpactData GetImpactData()
        {
            return new ImpactData(m_BulletData.OwnerCamp, 0, m_BulletData.Attack, 0);
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_BulletData = userData as BulletData;
            if (m_BulletData == null)
            {
                Log.Error("Bullet data is invalid.");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            DoHitscan();
            CachedTransform.Translate(Vector3.forward * m_BulletData.Speed * elapseSeconds, Space.Self);
        }

        /// <summary>
        /// 碰撞判定
        /// </summary>
        /// <param name="other"></param>
        private void DoHitscan()
        {
            RaycastHit hit;
            // 生成弹孔或者出血特效
            if (Physics.Raycast(CachedTransform.position, CachedTransform.forward, out hit, 10))
            {
                //Debug.Log("碰撞对象：" + hit.collider.name);
                if (hit.transform.tag == "Player")
                {
                    //  生成出血特效
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70001)
                    {
                        Position = hit.point,
                        Rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal)
                    });
                    Player injuredPlayer = hit.collider.GetComponent<Player>();
                    injuredPlayer._PlayerData.HP -= m_BulletData.Attack;
                    bool isKilled = false;
                    if(injuredPlayer._PlayerData.HP <= 0)
                    {
                        isKilled = true;
                    }

                    Log.Debug("我打到{0}了，造成了{1}点巨额伤害，对手还剩{2}滴血", injuredPlayer.Id, m_BulletData.Attack, injuredPlayer._PlayerData.HP);

                    SendHitMsg(injuredPlayer.Id, m_BulletData.Attack, isKilled);
                }
                else if(hit.transform.tag != "Empty")
                {
                    //  生成弹孔特效
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70001)
                    {
                        Position = hit.point,
                        Rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal)
                    });
                }
                GameEntry.Entity.HideEntity(this);
            }
        }

        public void SendHitMsg(int hitEntityId, int damage, bool isKilled)
        {
            DamageMsg msg = new DamageMsg();
            msg.id = GameEntry.Net.ID;
            msg.damage = damage;
            msg.injured = hitEntityId;
            msg.isKilled = isKilled;
            GameEntry.Net.Send(msg);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}
