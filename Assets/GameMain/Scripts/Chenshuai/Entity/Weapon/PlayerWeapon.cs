using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    #region 内部变量
    //武器射击范围
    public float range = 100f;

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
    public GameObject effObj;

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

    //武器伤害
    private int damage = 26;
    public int Damage
    {
        get { return damage; }
    }

    #endregion

    private void Start()
    {
        shootPoint = transform.Find("BulletStartPoint");
        currentBullects = bulletMag;
        reloadTimer = reloadRate;
        fireAudioSource = gameObject.GetComponent<AudioSource>();

        for(int i = 0; i < vectors.Length; i++)
        {
            excusions.Enqueue(vectors[i]);
        }
    }

    private void Update()
    {

        //开火计时器每帧增加
        if (fireTimer < fireRate)
        {
            fireTimer += Time.deltaTime;
        }

        //装弹计时器每帧增加
        if(reloadTimer < reloadRate)
        {
            reloadTimer += Time.deltaTime;
        }
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

        //控制武器射击速度，两者的差值就是枪械的设计间隔
        if (fireTimer < fireRate || currentBullects <= 0 || reloadTimer<reloadRate)
        {
            //TestArmAnimationController.ArmAnimator.SetTrigger("Fire");
            return;
        }
        
        //射线判定生成弹孔特效
        RaycastHit hit;
        shootDirection = shootPoint.forward + shootPoint.right*Random.Range(0,0.05f) + shootPoint.up*Random.Range(0, 0.05f);
        if(Physics.Raycast(shootPoint.position, shootDirection, out hit, range))
        {

            Instantiate(effObj, hit.point, Quaternion.FromToRotation(Vector3.up,hit.normal));
        }

        //依据发射方向创建子弹预设体
        GameObject obj = Instantiate(bullet, shootPoint.position, Quaternion.LookRotation(shootDirection));
        obj.GetComponent<Bullet_tmp>().ShootDirction = Quaternion.LookRotation(shootDirection);
        obj.GetComponent<Bullet_tmp>().SetFather(fatherObj);

        //创建并且播放特效
        ParticleSystem eff = Instantiate<ParticleSystem>(muzzleFlash, shootPoint.position, shootPoint.rotation);
        eff.Play();

        //播放射击音效
        fireAudioSource.Play();

        //控制子弹做什么
        Bullet_tmp bulletObj = obj.GetComponent<Bullet_tmp>();
        bulletObj.SetFather(fatherObj);

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

            //TestArmAnimationController.ArmAnimator.SetTrigger("Reload");
        }
        else
        {
            return;
        }
    }

}
