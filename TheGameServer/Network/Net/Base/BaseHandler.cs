using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ��Ϣ���������� ��Ҫ���ڴ�����Ϣ���߼���
/// </summary>
public abstract class BaseHandler 
{
    //�����ߴ����ĸ���Ϣ
    public BaseMsg message;

    //����������Ϣ�ķ���
    public abstract void MsgHandle();
}