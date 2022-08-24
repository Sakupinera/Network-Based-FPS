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
                if (hit.transform.tag == "Player")
                {
                    //  生成出血特效
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70002)
                    {
                        Position = hit.point,
                        Rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal)
                    });
                    Player injuredPlayer = hit.collider.GetComponent<Player>();
                    //玩家没有阵亡并阵容不同
                    if (injuredPlayer.playerStatus != E_PLAYER_STATUS.Die && m_BulletData.OwnerCamp != injuredPlayer.GetPlayerData.Camp)
                    {
                        injuredPlayer.GetPlayerData.HP -= m_BulletData.Attack;
                        bool isKilled = false;
                        if (injuredPlayer.GetPlayerData.HP <= 0)
                        {
                            isKilled = true;
                            GameEntry.Event.Fire(this, KillEvent.Create());
                        }

                        Log.Debug("我打到{0}了，造成了{1}点巨额伤害，对手还剩{2}滴血", injuredPlayer.Id, m_BulletData.Attack, injuredPlayer.GetPlayerData.HP);

                        SendHitMsg(injuredPlayer.Id, m_BulletData.Attack, isKilled);
                    }
                }
                else if (hit.transform.tag != "Empty")
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
