using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.Input��
/// 2.�¼�����ģ��
/// 3.����Monoģ���ʹ��
/// </summary>
public class InputMgr : BaseManager<InputMgr>
{
    private bool isStart = false;

    /// <summary>
    /// 开启upDate监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }

    /// <summary>
    /// 是否开启输入检查
    /// </summary>
    /// <param name="isOp"></param>
    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
        
    }

    /// <summary>
    /// 玩家按下鼠标触发
    /// </summary>
    /// <param name="i"></param>
    public void CheckMouseButtonDown(int i)
    {
        if (Input.GetMouseButton(i))
        {
            EventCenter.GetInstance().EventTrigger("鼠标按下" + i.ToString());
        }
        if (Input.GetMouseButtonUp(i))
        {
            EventCenter.GetInstance().EventTrigger("鼠标抬起" + i.ToString());
        }
    }

    /// <summary>
    /// 玩家按键触发
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
        if (Input.GetKey(key))
        {
            EventCenter.GetInstance().EventTrigger(key.ToString() + "按住", key);
        }
    }

    /// <summary>
    /// 玩家的移动水平逻辑
    /// </summary>
    /// <param name="axis"></param>
    private void CheckBodyAxis(string []axis)
    {
        EventCenter.GetInstance().EventTrigger<float[]>("水平移动", new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1])});
        TestPlayerAnimationContorller.PlayerAnimator.SetFloat("VelocityX", Input.GetAxis(axis[0]));
        TestPlayerAnimationContorller.PlayerAnimator.SetFloat("VelocityZ", Input.GetAxis(axis[1]));
    }

    /// <summary>
    /// 玩家的镜头逻辑
    /// </summary>
    /// <param name="axis"></param>
    private void CheckMouseAxis(string[] axis)
    {

        EventCenter.GetInstance().EventTrigger<float[]>("鼠标移动", new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1]) });
    }

    /// <summary>
    /// 玩家的按下Jump的逻辑
    /// </summary>
    /// <param name="button"></param>
    private void CheckButtonDown(string button)
    {
        if (Input.GetButtonDown(button))
        {
            EventCenter.GetInstance().EventTrigger<string>("垂直移动", button);
            TestPlayerAnimationContorller.PlayerAnimator.SetTrigger("Jump");
        }
        
    }

    /// <summary>
    /// 每帧进行调用
    /// </summary>
    private void MyUpdate()
    {
        if (!isStart)
        {
            return;
        }

        //每帧检查玩家的鼠标移动
        CheckMouseAxis(new string[2] { "Mouse X", "Mouse Y" });
        //每帧检查玩家是否跳跃按下
        CheckButtonDown("Jump");
        //每帧检测玩家是否开火
        CheckMouseButtonDown(0);
        //每帧检测玩家是否按下下蹲
        CheckKeyCode(KeyCode.LeftControl);
        //每帧检测玩家的移动
        CheckBodyAxis(new string[2] { "Horizontal", "Vertical" });
        //每帧检测玩家是否换弹
        CheckKeyCode(KeyCode.R);
        //每帧检测玩家是否开镜
        CheckMouseButtonDown(1);
        //每帧检测玩家是否按键切换枪支
        CheckKeyCode(KeyCode.Alpha1);
        CheckKeyCode(KeyCode.Alpha2);
        CheckKeyCode(KeyCode.Alpha3);
        CheckKeyCode(KeyCode.Alpha4);

    }
    
}
