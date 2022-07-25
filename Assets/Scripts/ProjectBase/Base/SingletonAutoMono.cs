using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C#中 泛型知识点
//设计模式 单例模式的知识点
//继承这种自动创建的 单例模式基类 不需要我们手动去拖 或者api去加了
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //继承了MonoBehaviour的脚本 不能够直接new
        //只能通过拖动到对象上 或者通过 加脚本的api AddComponent去加脚本
        //U3D内部帮助我们实例化它
        if (instance == null)
        {
            GameObject obj = new GameObject();
            //设置对象的名字为脚本名
            obj.name = typeof(T).ToString();

            //让这个单例模式对象 过场景 不移除
            //因为这个单例模式对象往往是存在整个声明周期中的
            DontDestroyOnLoad(obj);

            instance = obj.GetComponent<T>();
        }
        return instance;
    }
}
