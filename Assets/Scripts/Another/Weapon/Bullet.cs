using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //移动速度
    public float moveSpeed = 0.1f;
    
    //谁发射的我
    public PlayerObj fatherObj;

    //子弹销毁特效
    public GameObject effObj;
    public GameObject effObj2;
    void Start()
    {
        
    }


    void Update()
    {
        this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }


    public void SetFather(PlayerObj obj)
    {
        fatherObj = obj;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground"))
        {
            //当子弹销毁时，可以创建一个对象
            if(effObj != null)
            {
                GameObject eff = Instantiate(effObj,this.transform.position,this.transform.rotation);
                GameObject eff2 = Instantiate(effObj2, this.transform.position, this.transform.rotation);
            }

            Destroy(this.gameObject);
        }

    }
}
