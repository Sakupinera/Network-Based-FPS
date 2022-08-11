using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类
/// 找到自己面板下的所有控件对象
/// 他应该提供显示或者隐藏行为
/// 帮助我们通过代码快速找到所有的子控件
/// 方便我们在子类中处理逻辑
/// </summary>
public class BasePanel : MonoBehaviour
{
    //通过里氏替换原则 来存储所有的控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
        FindChildrenControl<InputField>();
    }

    /// <summary>
    /// 显示自己
    /// </summary>
    public virtual void ShowMe()
    {

    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void HideMe()
    {

    }

    //子类进行重写即可
    protected virtual void OnClick(string btnName)
    {

    }

    protected virtual void OnValueChanged(string toggleName,bool value)
    {

    }

    /// <summary>
    /// 找到子对象的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();
        
        for (int i = 0; i < controls.Length; i++)
        {
            string objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
            {
                controlDic[objName].Add(controls[i]);
            }
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });
            }

            //如果是按钮控件
            if (controls[i] is Button)
            {
                (controls[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            //如果是单选框控件
            else if (controls[i] is Toggle)
            {
                (controls[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnValueChanged(objName, value);
                });
            }
        }
    }

    /// <summary>
    /// 获得子对象的控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for(int i =0;i< controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)
                {
                    return controlDic[controlName][i] as T;
                }
            }
        }

        return null;
    }
}
