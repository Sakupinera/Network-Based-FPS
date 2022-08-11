using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ����ģ��
/// 1.�첽����
/// 2.ί�к�lambda���ʽ
/// 3.Э��
/// 4.����
/// </summary>
public class ResMgr : BaseManager<ResMgr>
{
    //ͬ��������Դ
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        //���������һ��GameObject���͵� �Ұ���ʵ������ �ٷ��س�ȥ �ⲿֱ�� ʹ�ü���
        if(res is GameObject)
        {
            return GameObject.Instantiate(res);
        }
        else //TextAsset AudioClip
        {
            return res;
        }
    }
    
    //�첽����
    public void LoadAsync<T>(string name,UnityAction<T> callback) where T:Object
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //������Эͬ������ ���� �����첽����
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if(r.asset is GameObject)
        {
            callback(GameObject.Instantiate(r.asset) as T);
        }
        else
        {
            callback(r.asset as T);
        }
    }
}
