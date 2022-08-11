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

            CachedTransform.Translate(Vector3.forward * m_BulletData.Speed * elapseSeconds, Space.Self);
        }

        /// <summary>
        /// 碰撞判定
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                //当子弹销毁时，可以创建一个火花特效
                GameEntry.Entity.HideEntity(this);
            }
            if (other.CompareTag("Player"))
            {
                //当子弹击中玩家时，可以创建一个出血特效

                GameEntry.Entity.HideEntity(this);
            }
        }
        //private void DoHitscan(Camera camera)
        //{
        //    RaycastHit hitInfo;

        //    Ray ray = camera.ViewportPointToRay(Vector2.one * 0.5f);

        //    if(Physics.Raycast(ray, out hitInfo, 50, LayerMask.NameToLayer("Ground"))){
        //        if(hitInfo.collider.gameObject.tag == "Player")
        //        {
        //            return;
        //        }
        //        //当子弹销毁时，可以创建一个弹孔特效
        //        GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), 70001)
        //        {
        //            Position = CachedTransform.position,
        //            Rotation = CachedTransform.rotation
        //        });
        //        GameEntry.Entity.HideEntity(this);
        //    }
        //}


        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }
    }
}
