using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承MonoBehaviour的基类
//C#中 泛型知识点
//设计模式 单例模式的知识点
//继承了MonoBehaviour的单例模式对象需要我们自己保证它的唯一性
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //继承了MonoBehaviour的脚本 不能够直接new
        //只能通过拖动到对象上 或者通过 加脚本的api AddComponent去加脚本
        //U3D内部帮助我们实例化它
        return instance;
    }

    //利于子类重写
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
