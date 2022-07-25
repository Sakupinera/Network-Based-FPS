using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : PlayerBaseObj
{
    public CharacterController controller;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);

        //�������ؼ�
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("GroundCheck");
        groundMask = LayerMask.GetMask("Ground");
        playerBody = this.transform;
        playerCame = transform.Find("MainCamera");

        //ͨ��InputMgr�����������
        InputMgr.GetInstance().StartOrEndCheck(true);
        EventCenter.GetInstance().AddEventListener<float[]>("ˮƽ�ƶ�", HorizontalMove);
        EventCenter.GetInstance().AddEventListener<string>("��ֱ�ƶ�", VerticalMove);
        EventCenter.GetInstance().AddEventListener("��갴��" + 0.ToString(),Shoot);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "����", Crouch);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "̧��", Crouch);
        EventCenter.GetInstance().AddEventListener<float[]>("����ƶ�", MouseMove);
    }

    
    void Update()
    {
        GravitySimulation();
    }


    /// <summary>
    /// ���ƽ�ɫ��ˮƽ�����ƶ��߼�, walk״̬
    /// </summary>
    /// <param name="num"></param>
    private void HorizontalMove(float[] num)
    {
        float x = num[0];
        float z = num[1];
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

    }

    /// <summary>
    /// �ڰ�����Ծ��ִ�е��߼�  jump״̬
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

    /// <summary>
    /// ���� LeftControl �����¶�
    /// </summary>
    private void Crouch(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //�����¶׶����������ƶ��ٶȣ�����ͷ�߶��½�


        }
        if (Input.GetKeyUp(key))
        {
            //LeftControļ��ȡ���¶׶��������һظ��ٶ�


        }
    }

    /// <summary>
    /// ����ģ��
    /// </summary>
    private void GravitySimulation()
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
    /// �ӽ��ƶ�
    /// </summary>
    /// <param name="nums"></param>
    private void MouseMove(float[] nums)
    {
        float mouseX = nums[0] * mouseSensitivity * Time.deltaTime;
        float mouseY = nums[1] * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// ��д���𷽷�
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Shoot()
    {
        if (weapon != null)
        {
            weapon.Fire();
        }
        
    }
}
