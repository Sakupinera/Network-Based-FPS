using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_tmp : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 0.1f;
    
    //谁发射的我
    public PlayerObj fatherObj;

    //子弹销毁特效
    public GameObject effObj;   //出血特效
    public GameObject effObj2;  //火花特效

    //发射方向
    public Quaternion ShootDirction;

    void Start()
    {

    }

    /// <summary>
    /// 发射子弹
    /// </summary>
    void Update()
    {
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

    }

    /// <summary>
    /// 设置谁发射的我
    /// </summary>
    /// <param name="obj"></param>
    public void SetFather(PlayerObj obj)
    {
        fatherObj = obj;
    }


    /// <summary>
    /// 碰撞判定
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            //当子弹销毁时，可以创建一个特效对象
            if(effObj2 != null)
            {
                //火花特效
                GameObject eff2 = Instantiate(effObj2, this.transform.position, this.transform.rotation);
            }
            Destroy(this.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            //当子弹销毁时，可以创建一个特效对象
            if (effObj != null)
            {
                //出血特效
                //GameObject eff = Instantiate(effObj, this.transform.position, this.transform.rotation);

                //触发碰撞对象的受伤效果(判断被击中的对象是不是自己)
                if (other.GetComponent<PlayerObj>()!= fatherObj)
                {
                    other.GetComponent<PlayerObj>().Wound(fatherObj);
                }
            }
            Destroy(this.gameObject);
        }

    }
}
