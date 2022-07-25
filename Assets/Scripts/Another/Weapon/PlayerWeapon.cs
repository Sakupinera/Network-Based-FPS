using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{

    //用于实例化的子弹对象
    public GameObject bullet;

    //范围
    public float range = 100f;

    //子弹总量
    public int bulletNum = 360;

    //一个弹夹的装弹量
    public int bulletMag = 30;

    //射击位置
    public Transform shootPoint;

    //武器拥有者
    public PlayerObj fatherObj;

    //枪口火焰特效
    public ParticleSystem muzzleFlash;


    private void Start()
    {
        shootPoint = transform.Find("BulletStartPoint");
    }

    public void SetFather(PlayerObj obj)
    {
        fatherObj = obj;
    }

    /// <summary>
    /// 开火方法
    /// </summary>
    public void Fire()
    {

        //创建子弹预设体
        GameObject obj = Instantiate(bullet, shootPoint.position, shootPoint.rotation);

        //创建并且播放特效
        ParticleSystem eff = Instantiate<ParticleSystem>(muzzleFlash, shootPoint.position, shootPoint.rotation);
        eff.Play();

        MusicMgr.GetInstance().PlaySound("shoot",gameObject);

        //控制子弹做什么
        Bullet bulletObj = obj.GetComponent<Bullet>();
        bulletObj.SetFather(fatherObj);

    }



}
