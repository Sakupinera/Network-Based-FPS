using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一定时间自动摧毁
public class AutoDestroy : MonoBehaviour
{
    public float time = 2;
    // Start is called before the first frame update
    void Start()
    {

        Destroy(gameObject,time);
    }


}
