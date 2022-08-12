using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBullet : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 10f;

    //谁发射的我
    public PlayerObj fatherObj;
    //投掷物销毁特效
    public GameObject bombObj;

    //发射方向
    public Quaternion ShootDirction;

    private Vector3 lastDir;
    private Rigidbody rigid;

    public int damage = 60;

    public E_ThrowType ThrowType;



    private void Start()
    {
        //自身控件获取
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = this.transform.forward * moveSpeed;
        //5秒后自动摧毁
        Invoke("Bomb", 4f);
    }

    private void LateUpdate()
    {
        lastDir = rigid.velocity;
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
    /// 爆炸逻辑
    /// </summary>
    public void Bomb()
    {
        //生成一个半径为10 的球体判定判断区域，将在该区域的所有tag为player的进行伤害计算
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, 10);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                //调用受伤方法
                colliders[i].GetComponent<PlayerBaseObj>().Wound(fatherObj);
            }
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 撞击反弹逻辑
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Ground")
        {
            //计算反弹方向
            Vector3 reflexAngle = Vector3.Reflect(lastDir, other.contacts[0].normal);
            //每次反弹以半数减少动量
            rigid.velocity = reflexAngle.normalized * lastDir.magnitude * 0.5f;
        }
    }
}
