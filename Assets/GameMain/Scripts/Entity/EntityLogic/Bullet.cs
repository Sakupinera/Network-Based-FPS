using UnityEngine;
using UnityGameFramework.Runtime;

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

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}
