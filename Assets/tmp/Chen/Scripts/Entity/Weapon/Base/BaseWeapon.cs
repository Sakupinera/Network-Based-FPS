using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_WeaponType
{
    Primary,
    Secondary,
    Close,
    Throw
}

public class BaseWeapon : MonoBehaviour
{
    //武器的id
    public int idWeapon;
    //武器的类型
    public E_WeaponType weaponType;
    //武器的范围
    public float range;
    //用于实例化的子弹对象
    public GameObject bullet;
    //子弹总量
    public int bulletNum = 360;
    //一个弹夹的装弹量
    public int bulletMag = 30;
    //当前子弹数量
    public int currentBullects;
    //射击位置
    public Transform shootPoint;
    //武器拥有者
    public PlayerObj fatherObj;
    //枪口火焰特效
    public ParticleSystem muzzleFlash;
    //枪械弹孔特效
    public GameObject effBulletHole;
    //射速
    public float fireRate = 0.1f;
    //装弹间隔
    public float reloadRate = 5f;
    //计时器
    //开火计时器
    public float fireTimer = 0;
    //装弹计时器
    public float reloadTimer = 0;
    //决定长按开火键多少时间算作连发
    public float cFireMaxTime = 1;
    //子弹发射方向
    public Vector3 shootDirection;
    //开火音效
    public AudioSource fireAudioSource;
    //弹道偏移值
    public Queue<Vector3> excusions = new Queue<Vector3>();
    protected Vector3[] vectors = new Vector3[] {new Vector3(-1f,0f,0),
                                              new Vector3(-1f,0f,0),
                                              new Vector3(-1f,0f,0),
                                              new Vector3(-2f,0f,0),
                                              new Vector3(-3f,0f,0),
                                              new Vector3(-3f,0f,0),
                                              new Vector3(0f,1f,0),
                                              new Vector3(0f,2f,0),
                                              new Vector3(0f,2f,0),
                                              new Vector3(0f,2f,0),
                                              new Vector3(0f,2f,0),
                                              new Vector3(0f,2f,0),
                                              new Vector3(0f,-1f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),                                              new Vector3(0f,-1f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0),
                                              new Vector3(0f,-2f,0)};
    //武器的伤害
    public int damage;

    protected virtual void Start()
    {
        //shootPoint = transform.Find("ShootStartPoint");
        currentBullects = bulletMag;
        reloadTimer = reloadRate;
        fireAudioSource = gameObject.GetComponent<AudioSource>();

        for (int i = 0; i < vectors.Length; i++)
        {
            excusions.Enqueue(vectors[i]);
        }
    }

    protected virtual void Update()
    {
        //开火计时器每帧增加
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        //装弹计时器每帧增加
        if (reloadTimer < reloadRate)
        {
            reloadTimer += Time.deltaTime;
        }
    }

    //设置父对象
    public virtual void SetFather(PlayerObj obj)
    {
        fatherObj = obj;
    }

    //开火逻辑
    public virtual void Fire()
    {

    }

    //换弹逻辑
    public virtual void ReloadBullet(KeyCode key)
    {

    }
}
