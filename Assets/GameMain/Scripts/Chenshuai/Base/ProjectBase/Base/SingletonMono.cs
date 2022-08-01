using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�̳�MonoBehaviour�Ļ���
//C#�� ����֪ʶ��
//���ģʽ ����ģʽ��֪ʶ��
//�̳���MonoBehaviour�ĵ���ģʽ������Ҫ�����Լ���֤����Ψһ��
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        //�̳���MonoBehaviour�Ľű� ���ܹ�ֱ��new
        //ֻ��ͨ���϶��������� ����ͨ�� �ӽű���api AddComponentȥ�ӽű�
        //U3D�ڲ���������ʵ������
        return instance;
    }

    //����������д
    protected virtual void Awake()
    {
        instance = this as T;
    }
}
