using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��ΪMono�Ĺ�����
/// 1.�������ں���
/// 2.�¼�
/// 3.Э��7
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
    /// ���ⲿ�ṩ�� ���֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// ���ⲿ�ṩ�� �Ƴ�֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}
