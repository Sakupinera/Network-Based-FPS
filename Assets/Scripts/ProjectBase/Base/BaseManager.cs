using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1.C#中 泛型的知识
//2.设计模式中 单例模式的知识
//减少单例模式中重复代码的出现
public class BaseManager<T> where T:new()
{
    /*一种简便写法
    private static T instance = new T();
    public static T Instance => instance;
     */
    private static T instance;

    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
        return instance;
    }

}
