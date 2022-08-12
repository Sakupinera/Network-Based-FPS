using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1.�����ṩ���ⲿ���֡�����¼��ķ���
/// 2.�����ṩ���ⲿ��� Э�̵ķ���
/// </summary>
public class MonoMgr : BaseManager<MonoMgr>
{
    private MonoController controller;

    public MonoMgr()
    {
        //��֤��MonoController�����Ψһ��
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// ���ⲿ�ṩ�� ���֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// ���ⲿ�ṩ�� �Ƴ�֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpateListener(UnityAction fun)
    {
        controller.RemoveUpateListener(fun);
    }

    /// <summary>
    /// �ṩ���ⲿ����Э�̵ķ���
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    /// <summary>
    /// �ṩ���ⲿ�ر�Э�̵ķ���
    /// </summary>
    /// <param name="routine"></param>
    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

}
