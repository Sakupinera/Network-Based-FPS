using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.Input类
/// 2.事件中心模块
/// 3.公共Mono模块的使用
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart = false;

    /// <summary>
    /// 构造函数中 添加update监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 是否开启或关闭 我的输入检测
    /// </summary>
    /// <param name="isOp"></param>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
        
    }

    public void CheckMouseButtonDown(int i)
    {
        if (Input.GetMouseButton(i))
        {
            EventCenter.GetInstance().EventTrigger("鼠标按下" + i.ToString());
        }
    }

    /// <summary>
    /// 用来检测按下抬起,分发事件的
    /// </summary>
    /// <param name="key"></param>
    private void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            EventCenter.GetInstance().EventTrigger(key.ToString() + "按下", key);
        }
        if (Input.GetKeyUp(key))
        {
            EventCenter.GetInstance().EventTrigger(key.ToString() + "抬起", key);
        }

    }

    /// <summary>
    /// 用于检测人物移动
    /// </summary>
    /// <param name="axis"></param>
    private void CheckBodyAxis(string []axis)
    {
        
        EventCenter.GetInstance().EventTrigger<float[]>("水平移动", new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1])}) ;
    }

    /// <summary>
    /// 用于检测鼠标移动
    /// </summary>
    /// <param name="axis"></param>
    private void CheckMouseAxis(string[] axis)
    {

        EventCenter.GetInstance().EventTrigger<float[]>("鼠标移动", new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1]) });
    }

    /// <summary>
    /// 用于检测按钮按下
    /// </summary>
    /// <param name="button"></param>
    private void CheckButtonDown(string button)
    {
        if (Input.GetButtonDown(button))
        {
            EventCenter.GetInstance().EventTrigger<string>("垂直移动", button);
        }
        
    }

    private void MyUpdate()
    {
        if (!isStart)
        {
            return;
        }
        CheckBodyAxis(new string[2] { "Horizontal" , "Vertical" });
        CheckMouseAxis(new string[2] { "Mouse X", "Mouse Y" });
        CheckButtonDown("Jump");
        CheckKeyCode(KeyCode.LeftShift);
        CheckMouseButtonDown(0);
    }
    
}
