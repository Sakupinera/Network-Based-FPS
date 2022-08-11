using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrimaryWeapon : BaseWeapon
{

    private void Awake()
    {
        //设置主武器的类型为 主武器
        weaponType = E_WeaponType.Primary;

    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {

        base.Update();
    }

    /// <summary>
    /// 设置武器拥有者
    /// </summary>
    /// <param name="obj"></param>
    public override void SetFather(PlayerObj obj)
    {
        fatherObj = obj;
    }

    /// <summary>
    /// 开火方法
    /// </summary>
    public override void Fire()
    {
        //用于存储实例化子弹对象
        GameObject obj;

        //控制武器射击速度，两者的差值就是枪械的设计间隔
        if (fireTimer < fireRate || currentBullects <= 0 || reloadTimer<reloadRate)
        {
            //TestArmAnimationController.ArmAnimator.SetTrigger("Fire");
            return;
        }

        //进行射击动画
        TestArmAnimationController.ArmAnimator.SetTrigger("Fire");

        //射线判定生成弹孔特效
        RaycastHit hit;
        shootDirection = shootPoint.forward + shootPoint.right * Random.Range(0, 0.05f) + shootPoint.up * Random.Range(0, 0.05f);
        if (Physics.Raycast(shootPoint.position, shootDirection, out hit, range))
        {
            if (hit.transform != null)
            {
                obj = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(shootDirection));
                obj.GetComponent<Bullet>().ShootDirction = Quaternion.LookRotation(shootDirection);
                obj.GetComponent<Bullet>().SetFather(fatherObj);
                //控制子弹做什么
                Bullet bulletObj = obj.GetComponent<Bullet>();
                bulletObj.SetFather(fatherObj);
            }

            Instantiate(effBulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        }


        //创建并且播放特效
        ParticleSystem eff = Instantiate<ParticleSystem>(muzzleFlash, shootPoint.position, shootPoint.rotation);
        eff.Play();

        //播放射击音效
        fireAudioSource.Play();

        //当前子弹减少
        currentBullects--;

        //重置开火计时器
        fireTimer = 0;

    }

    /// <summary>
    /// 换弹逻辑
    /// </summary>
    /// <param name="key"></param>
    public override void ReloadBullet(KeyCode key)
    {
        if (Input.GetKeyDown(KeyCode.R) && currentBullects < bulletMag && bulletNum > 0)
        {
            //计算出当前子弹数补满一个弹夹需要的的剩余子弹
            int bulletNeed = bulletMag - currentBullects;

            if (bulletNum >= bulletNeed)
            {
                bulletNum -= bulletNeed;
                currentBullects += bulletNeed;
            }else if(bulletNum < bulletNeed)
            {
                currentBullects += bulletNum;
                bulletNum -= bulletNum;
            }
            reloadTimer = 0;

            TestArmAnimationController.ArmAnimator.SetTrigger("Reload");
        }
        else
        {
            return;
        }
    }

}
