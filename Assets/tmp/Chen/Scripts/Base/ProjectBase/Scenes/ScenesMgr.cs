using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenesMgr : BaseManager<ScenesMgr>
{
    /// <summary>
    /// �л����� ͬ��
    /// </summary>
    /// <param name="name"></param>
    public void LoadScene(string name,UnityAction fun)
    {
        //����ͬ������
        SceneManager.LoadScene(name);
        //����������󣬲Ż�ִ��fun
    }

    /// <summary>
    /// �ṩ���ⲿ�� �첽���صĽӿڷ���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsyn(string name,UnityAction fun)
    {
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadSceneAsyn(name, fun));
    }

    /// <summary>
    /// Э���첽���س���
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsyn(string name,UnityAction fun)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //���Եõ��������ص�һ����������
        while (!ao.isDone)
        {
            EventCenter.GetInstance().EventTrigger("����������", ao.progress);
            //������ȥ���½�����
            yield return ao.progress;
        }
        //������󣬲Ż�ִ��
        fun();
    }
}
