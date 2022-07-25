using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region �ڲ�����
    public CharacterController controller;

    //������������·�ͱ����ٶ��м�任�� ʵʱ�ٶ�
    public float midSpeed = 4f;

    //��·�ٶ�
    public float speed = 4f;

    public float gravity = -9.81f;

    //�����ٶ�
    public float runSpeed = 6f;

    //һ�����ڽ��е����жϵı���
    public Transform groundCheck;
    public float groundDistence = 0.4f;
    public LayerMask groundMask;

    //��Ծʱ���ܴﵽ�������ٶ�
    public float jumpHeight = 1.5f;

    Vector3 velocity;

    //�ж��Ƿ��ڵ���
    bool isGrounded;
    #endregion

    void Start()
    {

        //ͨ��InputMgr�����������
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<float[]>("ˮƽ�ƶ�", HorizontalMove);
        EventCenter.GetInstance().AddEventListener<string>("��ֱ�ƶ�",VerticalMove);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftShift.ToString() + "����", Run);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftShift.ToString() + "̧��", Run);
    }

    /// <summary>
    /// update��Ҫ���������ֱ������ƶ��߼�(����ģ��)
    /// </summary>
    void Update()
    {

        //Physics.CheckSpher��һ��ָ��λ�ô���һ��ָ���뾶�����壬����ָ���Ĳ㼶�����������ײ�ж�
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }


    /// <summary>
    /// ����
    /// </summary>
    /// <param name="key">���µļ�λ</param>
    private void Run(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            midSpeed = runSpeed;
        }
        if (Input.GetKeyUp(key))
        {
            midSpeed = speed;
        }
    }

    /// <summary>
    /// ���ƽ�ɫ��ˮƽ�����ƶ��߼�
    /// </summary>
    /// <param name="num"></param>
    private void HorizontalMove(float[]num)
    {
        float x = num[0];
        float z = num[1];
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * midSpeed * Time.deltaTime);

    }

    /// <summary>
    /// �ڰ�����Ծ��ִ�е��߼�
    /// </summary>
    /// <param name="button"></param>
    private void VerticalMove(string button)
    {
        //Physics.CheckSpher��һ��ָ��λ�ô���һ��ָ���뾶�����壬����ָ���Ĳ㼶�����������ײ�ж�
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
        if (Input.GetButtonDown(button) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }


}
