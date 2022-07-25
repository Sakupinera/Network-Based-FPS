using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 作为Mono的管理者
/// 1.生命周期函数
/// 2.事件
/// 3.协程7
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(updateEvent != null)
        {
            updateEvent();
        }
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 给外部提供的 移除帧更新事件的函数
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
