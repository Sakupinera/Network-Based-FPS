using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : PlayerBaseObj
{
    float mouseX;
    float mouseY;

    public CharacterController controller;

    //按下开火键的累计时间
    public float cFireTime = 0;

    /// <summary>
    /// 构造函数，初始化角色的各种属性
    /// </summary>
    public PlayerObj()
    {
        this.hp = 100;
        this.speed = 4f;
        this.maxHp = 100;
    }

    void Start()
    {
        //鼠标设定相关
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);

        //获得自身控件
        controller = GetComponent<CharacterController>();
        walkAS = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");
        playerCame = transform.Find("MainCamera");
        groundMask = LayerMask.GetMask("Ground");
        playerBody = this.transform;
       

        //获取武器组件，并设置父亲对象为自己
        if (playerWeapon != null)
        {
            weapon = playerWeapon.GetComponent<PlayerWeapon>();
            weapon.SetFather(this);
        }
        
        //通过InputMgr进行输入侦测
        InputMgr.GetInstance().StartOrEndCheck(true);
        //通过事件中心注册
        EventCenter.GetInstance().AddEventListener<float[]>("水平移动", HorizontalMove);
        EventCenter.GetInstance().AddEventListener<string>("垂直移动", VerticalMove);
        EventCenter.GetInstance().AddEventListener("鼠标按下" + 0.ToString(),Shoot);
        EventCenter.GetInstance().AddEventListener("鼠标抬起" + 0.ToString(), Shoot);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "按下", Crouch);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "抬起", Crouch);
        EventCenter.GetInstance().AddEventListener<float[]>("鼠标移动", MouseMove);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.R.ToString() + "按下", ReloadBullet);
        EventCenter.GetInstance().AddEventListener("鼠标按下" + 1.ToString(), Aim);
        EventCenter.GetInstance().AddEventListener("鼠标抬起" + 1.ToString(), Aim);

    }

    //每帧进行重力检测
    void Update()
    {
        //每帧进行重力检测
        GravitySimulation();
    }


    /// <summary>
    /// 控制角色在水平地面移动逻辑, walk状态
    /// </summary>
    /// <param name="num"></param>
    private void HorizontalMove(float[] num)
    {
        float x = num[0];
        float z = num[1];

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        //后续做Crouch动作时，再额外加上蹲下静音的判断
        //在地面上 且有速度变化，播放走路音效
        if (move.sqrMagnitude > 0.9f && isGrounded)
        {
            //走路音效未播放
            if (!walkAS.isPlaying)
            {
                //播放
                walkAS.Play();
            }
        }
        else
        {
            //走路音效播放中
            if (walkAS.isPlaying)
            {
                //暂停
                walkAS.Pause();
            }
            
        }

        
    }

    /// <summary>
    /// 在按下跳跃键执行的逻辑  jump状态
    /// </summary>
    /// <param name="button"></param>
    private void VerticalMove(string button)
    {
        //Physics.CheckSpher在一个指定位置创建一个指定半径的球体，并与指定的层级的物体进行碰撞判断
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
        if (Input.GetButtonDown(button) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerStats = PlayerStats.JUMP;
        }
    }

    /// <summary>
    /// 按下 LeftControl 进行下蹲
    /// </summary>
    private void Crouch(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //播放下蹲动画，降低移动速度，摄像头高度下降
            TestPlayerAnimationContorller.PlayerAnimator.SetTrigger("Crouch");
        }
        if (Input.GetKeyUp(key))
        {
            //LeftControl抬起取消下蹲动画，并且回复速度
        }
    }

    /// <summary>
    /// 重力模拟
    /// </summary>
    private void GravitySimulation()
    {
        //Physics.CheckSpher在一个指定位置创建一个指定半径的球体，并与指定的层级的物体进行碰撞判断
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// 视角移动
    /// </summary>
    /// <param name="nums"></param>
    private void MouseMove(float[] nums)
    {
        mouseX = nums[0] * mouseSensitivity * Time.deltaTime;
        mouseY = nums[1] * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        
    }

    /// <summary>
    /// 重写开火方法
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Shoot()
    {
        //武器执行开火行为
        if (weapon != null)
        {
            weapon.Fire();
        }

        //进行弹道偏移判断
        if (Input.GetMouseButton(0) && weapon.currentBullects > 0 && weapon.reloadTimer >= weapon.reloadRate)
        {
            cFireTime += Time.deltaTime;
            if(cFireTime >= weapon.cFireMaxTime)
            {
                cFireTime = 0f;
                Vector3 vector = weapon.excusions.Dequeue();
                xRotation += vector.x;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * vector.y);
                weapon.excusions.Enqueue(vector);
            }
        }
        //鼠标抬起将累计时间清零
        if (Input.GetMouseButtonUp(0))
        {
            cFireTime = 0f;
        }
    }

    /// <summary>
    /// 填充弹药  Weapon对象来实现
    /// </summary>
    public void ReloadBullet(KeyCode key)
    {
        weapon.ReloadBullet(key);
    }

    /// <summary>
    /// 开镜逻辑
    /// </summary>
    public void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (weapon != null)
            {
                //WeaponCamera.transform.localPosition = new Vector3(0, 0, 0);
                //PlayerCamera.fieldOfView = 30;
                //TestArmAnimationController.ArmAnimator.SetFloat("Fire Index", 1);

                StartCoroutine(AimTransform(0, playerWeapon.transform.localPosition, new Vector3(-0.083f, 0.098f, -0.115f)));
                //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(-0.923f, 3.337f, -0.954f)));
                playerWeapon.transform.localRotation = Quaternion.Euler(new Vector3(-0.923f, 3.337f, -0.954f));
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(AimTransform(0, playerWeapon.transform.localPosition, new Vector3(-0.012f, 0.012f, 0f)));
            //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(0f, 0f, 0f)));
            playerWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
    }

    IEnumerator AimTransform(int i, Vector3 origin, Vector3 target)
    {
        float timer = 0f;
        while (timer  <= 1)
        {
            timer += Time.deltaTime*5;
            if (i == 0)
            {
                playerWeapon.transform.localPosition = Vector3.Lerp(origin, target, timer);
            }else if (i == 1)
            {
                //playerWeapon.transform.localRotation = Quaternion.Euler(Vector3.Lerp(origin, target, timer));
            }

            yield return null;
        }
    }

    /// <summary>
    /// 受伤逻辑
    /// </summary>
    /// <param name="other"></param>
    public override void Wound(PlayerBaseObj other)
    {
        base.Wound(other);
    }

    /// <summary>
    /// 玩家死亡逻辑
    /// </summary>
    public override void Dead()
    {
        base.Dead();
    }
}
