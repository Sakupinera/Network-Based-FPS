using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///抽屉数据，池子中的一列容器
/// </summary>
public class PoolData
{
    public GameObject fatherObj;
    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject poolObj)
    {
        //给我们的抽屉创建一个父对象，并且把他作为我们父对象的子物体
        fatherObj = new GameObject(obj.name);
        fatherObj.transform.parent = poolObj.transform;

        poolList = new List<GameObject>() { obj};
        PushObj(obj);
    }

    /// <summary>
    /// 往外拿东西
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject GetObj(string name)
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        //拿出后需要激活物体
        obj.SetActive(true);
        //断开父子关系
        obj.transform.parent = null;

        return obj;
    }

    /// <summary>
    /// 往抽屉里面压东西
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(GameObject obj)
    {
        //放进去后需要失活物体
        obj.SetActive(false);
        //存起来然后设置父对象
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
}

//缓存池模块
//1.Dictionary List
//2.GameObject 和 Resources 两个公共类中的API
//3.减少CPU和内存开销，减少GC次数
public class PoolMgr : BaseManager<PoolMgr>
{
    private GameObject poolObj;

    //缓存池容器（衣柜）
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    /// <summary>
    /// 往外拿东西
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void ObjGet(string name,UnityAction<GameObject> callBack)
    {
        
        //有抽屉 并且抽屉里有东西可以拿
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count>0)
        {
            
            callBack(poolDic[name].GetObj(name));
        }
        else
        {
            //通过异步加载资源，创建对象给外部用
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (o) =>
            {
                o.name = name;
                callBack(o);
            });
            //obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
            //把对象的名字改成和缓存池的名字一样
            //obj.name = name;
        }
        
    }

    /// <summary>
    /// 换暂时不用的东西给我
    /// </summary>
    /// <param name="name"></param>
    /// <param name="obj"></param>
    public void PushObj(string name,GameObject obj)
    {
        if(poolObj == null)
        {
            poolObj = new GameObject("Pool");
        }

        //设置父对象
        obj.transform.parent = poolObj.transform;

        //放进去后需要失活物体
        obj.SetActive(false);

        //里面有抽屉
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        //里面没有抽屉
        else
        {
            poolDic.Add(name, new PoolData(obj,poolObj));
        }

        
    }
    
    /// <summary>
    /// 清空缓存池的方法，主要是过场景时使用
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        poolObj = null;
    }
}
