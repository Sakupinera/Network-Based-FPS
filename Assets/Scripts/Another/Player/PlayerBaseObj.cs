using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseObj : MonoBehaviour
{
    //��ǰѪ�������Ѫ�����
    public int maxHp;
    public int hp;

    //�ٶ�
    public float speed = 4f;

    //����
    public PlayerWeapon weapon;

    // ����
    public float gravity = -9.81f;

    //һ�����ڽ��е����жϵı���
    public Transform groundCheck;
    public float groundDistence = 0.4f;
    public LayerMask groundMask;

    //��Ծʱ���ܴﵽ�������ٶ�
    public float jumpHeight = 1.5f;

    public Vector3 velocity;

    //�ж��Ƿ��ڵ���
    public bool isGrounded;

    public float mouseSensitivity = 100f;
    public Texture2D tex;
    public Transform playerBody;
    public Transform playerCame;

    public float xRotation = 0f;

    /// <summary>
    /// ���𷽷�
    /// </summary>
    public abstract void Shoot();
    
    /// <summary>
    /// ���˷���
    /// </summary>
    /// <param name="other"></param>
    public virtual void Wound(PlayerBaseObj other)
    {

    }

    /// <summary>
    /// ��������
    /// </summary>
    public virtual void Dead()
    {
        //�����������������


        //�����������ӳ������Ƴ��ö���
        Destroy(this.gameObject);
        

    }
}
