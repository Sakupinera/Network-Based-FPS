using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//UI的各个层级
public enum E_UI_Layer 
{
    Bot,
    Mid,
    Top,
    System
}

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部 显示和隐藏接口等
/// </summary>
public class UIManager : MonoBehaviour
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    //记录我们UI的Canvas父对象 方面以后外部可能会使用它
    public RectTransform canvas;

    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public UIManager()
    {
        //创建Canvas 让其过场景不会移除
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        //找到各层
        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //创建EventSystem 让其过场景不会移除
        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);

    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">显示在哪一层</param>
    /// <param name="callBack">当面板预设体创建成功后 你想做的事</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            //处理面板创建完成后的逻辑
            if (callBack != null)
            {
                callBack(panelDic[panelName] as T);
            }
            //避免面板重复加载，如果存在该面板 即直接显示 调用回调函数后 直接return 不再处理后面的异步加载逻辑
            return;
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //把他作为 Canvas的子对象
            //并且 要设置他的相对位置
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的面板脚本
            T panel = obj.GetComponent<T>();
            //处理面板创建完成后的逻辑
            if (callBack != null)
            {
                callBack(panel);
            }
            //创建完后添加进入字典
            panelDic.Add(panelName, panel);

        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="PanelName">面板的名字</param>
    public void HidePanle(string PanelName)
    {
        //如果包含面板的名字
        if (panelDic.ContainsKey(PanelName))
        {
            //摧毁物体
            GameObject.Destroy(panelDic[PanelName].gameObject);
            //并从字典记录中移除
            panelDic.Remove(PanelName);
        }
    }

    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }

        return null;
    }

    /// <summary>
    /// 得到层级
    /// </summary>
    /// <param name="layer"></param>
    /// <returns></returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bot:
                return this.bot;
            case E_UI_Layer.Mid:
                return this.mid;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 给控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件的响应函数</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type,UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }

}