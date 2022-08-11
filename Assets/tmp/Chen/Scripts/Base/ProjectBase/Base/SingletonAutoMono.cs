using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//C#�� ����֪ʶ��
//���ģʽ ����ģʽ��֪ʶ��
//�̳������Զ������� ����ģʽ���� ����Ҫ�����ֶ�ȥ�� ����apiȥ����
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //�̳���MonoBehaviour�Ľű� ���ܹ�ֱ��new
        //ֻ��ͨ���϶��������� ����ͨ�� �ӽű���api AddComponentȥ�ӽű�
        //U3D�ڲ���������ʵ������
        if (instance == null)
        {
            GameObject obj = new GameObject();
            //���ö��������Ϊ�ű���
            obj.name = typeof(T).ToString();

            //���������ģʽ���� ������ ���Ƴ�
            //��Ϊ�������ģʽ���������Ǵ����������������е�
            DontDestroyOnLoad(obj);

            instance = obj.GetComponent<T>();
        }
        return instance;
    }
}
