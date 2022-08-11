using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///�������ݣ������е�һ������
/// </summary>
public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        //�����ǵĳ��봴��һ�������󣬲��Ұ�����Ϊ���Ǹ������������
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;

        poolList = new List<GameObject>() { obj};
        PushObj(obj);
    }

    /// <summary>
    /// �����ö���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        //�ó�����Ҫ��������
        obj.SetActive(true);
        //�Ͽ����ӹ�ϵ
        obj.transform.parent = null;

        return obj;
    }

    /// <summary>
    /// ����������ѹ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        //�Ž�ȥ����Ҫʧ������
        obj.SetActive(false);
        //������Ȼ�����ø�����
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
}

//�����ģ��
//1.Dictionary List
//2.GameObject �� Resources �����������е�API
//3.����CPU���ڴ濪��������GC����
public class PoolMgr : BaseManager<PoolMgr>
{
    private GameObject poolObj;

    //������������¹�
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    /// <summary>
    /// �����ö���
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void ObjGet(string name,UnityAction<GameObject> callBack)
    {
        
        //�г��� ���ҳ������ж���������
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {
            
            callBack(poolDic[name].GetObj(name));
        }
        else
        {
            //ͨ���첽������Դ������������ⲿ��
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callBack(o);
            });
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //�Ѷ�������ָĳɺͻ���ص�����һ��
            //obj.name = name;
        }
        
    }

    /// <summary>
    /// ����ʱ���õĶ�������
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(string name,GameObject obj)
    {
        if(poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }

        //���ø�����
        obj.transform.parent = poolObj.transform;

        //�Ž�ȥ����Ҫʧ������
        obj.SetActive(false);

        //�����г���
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //����û�г���
        else
        {
            poolDic.Add(name, new PoolData(obj,poolObj));
        }

        
    }
    
    /// <summary>
    /// ��ջ���صķ�������Ҫ�ǹ�����ʱʹ��
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
