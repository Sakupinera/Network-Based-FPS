using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : PlayerBaseObj
{
    float mouseX;
    float mouseY;


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
        #region 鼠标相关设定
        //鼠标设定相关
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
        #endregion

        #region 自身控件获取
        //获得自身控件
        controller = GetComponent<CharacterController>();
        walkAS = GetComponent<AudioSource>();
        groundCheck = transform.Find("GroundCheck");
        playerCame = transform.Find("MainCamera");
        groundMask = LayerMask.GetMask("Ground");
        playerBody = this.transform;

        if (currentPlayerWeapon != null)
        {
            currentWeapon = currentPlayerWeapon.GetComponent<BaseWeapon>();
        }
        #endregion

        #region 测试玩家本身已经挂载武器，通过下面方法将武器存储到武器字典中,并且将玩家初始武器设置为近战武器
        //初始化自身武器
        FindWeapon<PrimaryWeapon>();
        FindWeapon<SecondaryWeapon>();
        FindWeapon<CloseWeapon>();
        FindWeapon<ThrowWeapon>();
        #endregion


        #region 通过输入中心，侦测玩家的按键输入
        //通过InputMgr进行输入侦测
        InputMgr.GetInstance().StartOrEndCheck(true);
        //通过事件中心注册
        EventCenter.GetInstance().AddEventListener<float[]>("水平移动", HorizontalMove);
        EventCenter.GetInstance().AddEventListener<string>("垂直移动", VerticalMove);
        EventCenter.GetInstance().AddEventListener("鼠标按下" + 0.ToString(), Shoot);
        EventCenter.GetInstance().AddEventListener("鼠标抬起" + 0.ToString(), Shoot);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "按下", Crouch);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.LeftControl.ToString() + "抬起", Crouch);
        EventCenter.GetInstance().AddEventListener<float[]>("鼠标移动", MouseMove);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.R.ToString() + "按下", ReloadBullet);
        EventCenter.GetInstance().AddEventListener("鼠标按下" + 1.ToString(), Aim);
        EventCenter.GetInstance().AddEventListener("鼠标抬起" + 1.ToString(), Aim);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.Alpha1.ToString() + "按下", WeaponSwap);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.Alpha2.ToString() + "按下", WeaponSwap);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.Alpha3.ToString() + "按下", WeaponSwap);
        EventCenter.GetInstance().AddEventListener<KeyCode>(KeyCode.Alpha4.ToString() + "按下", WeaponSwap);
        #endregion

    }

    //每帧进行重力检测
    void Update()
    {
        //每帧进行重力检测
        GravitySimulation();
    }

    #region 控制角色水平移动逻辑
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
        if (move.sqrMagnitude > 0.9f && isGrounded && playerStats != PlayerStats.CROUCH)
        {
            //走路音效未播放
            if (!walkAS.isPlaying)
            {
                //播放
                walkAS.Play();
                //设置玩家状态
                playerStats = PlayerStats.WALK;
            }
        }
        else
        {
            //走路音效播放中
            if (walkAS.isPlaying)
            {
                //暂停
                walkAS.Pause();
                //设置玩家状态--走路->站立不动
                playerStats = PlayerStats.IDLE;
            }
            
        }

        
    }
    #endregion

    #region 控制角色数值移动逻辑
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
    #endregion

    #region 下蹲逻辑
    /// <summary>
    /// 按下 LeftControl 进行下蹲
    /// </summary>
    private void Crouch(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            //播放下蹲动画，降低移动速度，摄像头高度下降
            TestPlayerAnimationContorller.PlayerAnimator.SetTrigger("Crouch");
            StartCoroutine(CrouchTransform(0, PlayerCamera.transform.localPosition, new Vector3(0,0.3f, 0.347f)));
            controller.center = new Vector3(0, -0.5f, 0);
            controller.height = 0.5f;

            
        }
        if (Input.GetKeyUp(key))
        {
            //LeftControl抬起取消下蹲动画，并且回复速度
            TestPlayerAnimationContorller.PlayerAnimator.SetTrigger("Crouch");
            StartCoroutine(CrouchTransform(0, PlayerCamera.transform.localPosition, new Vector3(0, 0.7f, 0.347f)));
            controller.center = new Vector3(0, 0, 0);
            controller.height = 2f;
        }
    }

    IEnumerator CrouchTransform(int i, Vector3 origin, Vector3 target)
    {
        float timer = 0f;
        while (timer <= 1)
        {
            timer += Time.deltaTime * 5;
            if (i == 0)
            {
                PlayerCamera.transform.localPosition = Vector3.Lerp(origin, target, timer);
            }
            else if (i == 1)
            {
                //playerWeapon.transform.localRotation = Quaternion.Euler(Vector3.Lerp(origin, target, timer));
            }

            yield return null;
        }
    }
    #endregion

    #region 重力逻辑
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
    #endregion

    #region 鼠标视角移动逻辑
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
    #endregion  

    #region 一组与武器相关的逻辑
    /// <summary>
    /// 重写开火方法
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public override void Shoot()
    {
        //武器执行开火行为
        if (currentWeapon != null)
        {
            currentWeapon.Fire();
            Debug.Log("开火");
        }
        
        //进行弹道偏移判断
        if (Input.GetMouseButton(0) && currentWeapon.currentBullects > 0 && currentWeapon.reloadTimer >= currentWeapon.reloadRate)
        {
            cFireTime += Time.deltaTime;
            if(cFireTime >= currentWeapon.cFireMaxTime)
            {
                cFireTime = 0f;
                Vector3 vector = currentWeapon.excusions.Dequeue();
                xRotation += vector.x;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                playerBody.Rotate(Vector3.up * vector.y);
                currentWeapon.excusions.Enqueue(vector);
            }
        }//鼠标抬起将累计时间清零
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
        currentWeapon.ReloadBullet(key);
    }
   

    
    /// <summary>
    /// 开镜逻辑
    /// </summary>
    public void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentWeapon != null)
            {
                //WeaponCamera.transform.localPosition = new Vector3(0, 0, 0);
                //PlayerCamera.fieldOfView = 30;
                //TestArmAnimationController.ArmAnimator.SetFloat("Fire Index", 1);

                StartCoroutine(AimTransform(0, currentPlayerWeapon.transform.localPosition, new Vector3(-0.083f, 0.098f, -0.115f),78f,40f));

                //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(-0.923f, 3.337f, -0.954f)));
                currentPlayerWeapon.transform.localRotation = Quaternion.Euler(new Vector3(-0.923f, 3.337f, -0.954f));
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            StartCoroutine(AimTransform(0, currentPlayerWeapon.transform.localPosition, new Vector3(-0.012f, 0.012f, 0f),40f,78f));
            //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(0f, 0f, 0f)));
            currentPlayerWeapon.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
    }

    IEnumerator AimTransform(int i, Vector3 origin, Vector3 target,float a,float b)
    {
        float timer = 0f;
        while (timer  <= 1)
        {
            timer += Time.deltaTime*5;
            if (i == 0)
            {
                currentPlayerWeapon.transform.localPosition = Vector3.Lerp(origin, target, timer);
                PlayerCamera.fieldOfView = Mathf.Lerp(a, b, timer);
            }else if (i == 1)
            {
                //playerWeapon.transform.localRotation = Quaternion.Euler(Vector3.Lerp(origin, target, timer));
            }

            yield return null;
        }
    }
    

    

    /// <summary>
    /// 玩家切枪逻辑
    /// </summary>
    public void WeaponSwap(KeyCode key)
    {
        StartCoroutine(RealWeaponSwap(key));
    }

    /// <summary>
    /// 玩家切枪逻辑
    /// </summary>
    private IEnumerator RealWeaponSwap(KeyCode key)
    {
        //如果按下的是数字键盘1
        if (key == KeyCode.Alpha1)
        {
            //如果字典中包含主武器键
            if (WeaponDic.ContainsKey(E_WeaponType.Primary) && currentPlayerWeapon != WeaponDic[E_WeaponType.Primary])
            {
                //判定成功：
                //播放武器动画


                //播放动画所需要的时间
                yield return new WaitForSeconds(1f);
                //武器切换
                //先让玩家当前武器物体失活
                currentPlayerWeapon.SetActive(false);
                //然后从玩家的武器物体的剩余实体中取出武器设置当前武器实体
                //这里假设所里有武器都已经在购买的时候已经完成实例化，就是武器已经在场景中出现
                currentPlayerWeapon = WeaponDic[E_WeaponType.Primary];
                currentPlayerWeapon.SetActive(true);
                currentPlayerWeapon.GetComponent<BaseWeapon>().enabled = true;
                //得到玩家当前武器的脚本
                currentWeapon = currentPlayerWeapon.GetComponent<BaseWeapon>();
            }

        }
        if (key == KeyCode.Alpha2)
        {
            //如果字典中包含主武器键
            if (WeaponDic.ContainsKey(E_WeaponType.Secondary) && currentPlayerWeapon != WeaponDic[E_WeaponType.Secondary])
            {
                //判定成功：
                //播放武器动画


                //播放动画所需要的时间
                yield return new WaitForSeconds(1f);
                //武器切换
                //先让玩家当前武器物体失活
                currentPlayerWeapon.SetActive(false);
                //然后从玩家的武器物体的剩余实体中取出武器设置当前武器实体
                //这里假设所里有武器都已经在购买的时候已经完成实例化，就是武器已经在场景中出现
                currentPlayerWeapon = WeaponDic[E_WeaponType.Secondary];
                currentPlayerWeapon.SetActive(true);
                currentPlayerWeapon.GetComponent<BaseWeapon>().enabled = true;
                //得到玩家当前武器的脚本
                currentWeapon = currentPlayerWeapon.GetComponent<BaseWeapon>();
            }

        }
        if (key == KeyCode.Alpha3)
        {
            //如果字典中包含主武器键
            if (WeaponDic.ContainsKey(E_WeaponType.Close) && currentPlayerWeapon != WeaponDic[E_WeaponType.Close])
            {
                //判定成功：
                //播放武器动画


                //播放动画所需要的时间
                yield return new WaitForSeconds(1f);
                //武器切换
                //先让玩家当前武器物体失活
                currentPlayerWeapon.SetActive(false);
                //然后从玩家的武器物体的剩余实体中取出武器设置当前武器实体
                //这里假设所里有武器都已经在购买的时候已经完成实例化，就是武器已经在场景中出现
                currentPlayerWeapon = WeaponDic[E_WeaponType.Close];
                currentPlayerWeapon.SetActive(true);
                currentPlayerWeapon.GetComponent<BaseWeapon>().enabled = true;
                //得到玩家当前武器的脚本
                currentWeapon = currentPlayerWeapon.GetComponent<BaseWeapon>();
            }

        }
        if (key == KeyCode.Alpha4)
        {
            //如果字典中包含主武器键
            if (WeaponDic.ContainsKey(E_WeaponType.Throw) && currentPlayerWeapon != WeaponDic[E_WeaponType.Throw] && WeaponDic[E_WeaponType.Throw].GetComponent<ThrowWeapon>().throws2.Count != 0)
            {
                //判定成功：
                //播放武器动画


                //播放动画所需要的时间
                yield return new WaitForSeconds(1f);
                //武器切换
                //先让玩家当前武器物体失活
                currentPlayerWeapon.SetActive(false);
                //然后从玩家的武器物体的剩余实体中取出武器设置当前武器实体
                //这里假设所里有武器都已经在购买的时候已经完成实例化，就是武器已经在场景中出现
                currentPlayerWeapon = WeaponDic[E_WeaponType.Throw];
                currentPlayerWeapon.SetActive(true);
                currentPlayerWeapon.GetComponent<BaseWeapon>().enabled = true;
                //得到玩家当前武器的脚本
                currentWeapon = currentPlayerWeapon.GetComponent<BaseWeapon>();
            }
            if (currentPlayerWeapon == WeaponDic[E_WeaponType.Throw])
            {
                ThrowWeapon throwWeapon = currentPlayerWeapon.GetComponent<ThrowWeapon>();
                if (throwWeapon.grenade.ThrowType == E_ThrowType.Antitank)
                {
                    if (throwWeapon.throws2.ContainsKey(E_ThrowType.Smoke))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Smoke];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Smoke].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                    else if (throwWeapon.throws2.ContainsKey(E_ThrowType.Flash))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Flash];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Flash].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                }
                if (throwWeapon.grenade.ThrowType == E_ThrowType.Smoke)
                {
                    if (throwWeapon.throws2.ContainsKey(E_ThrowType.Antitank))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Smoke];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Smoke].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                    else if (throwWeapon.throws2.ContainsKey(E_ThrowType.Flash))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Flash];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Flash].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                }
                if (throwWeapon.grenade.ThrowType == E_ThrowType.Flash)
                {
                    if (throwWeapon.throws2.ContainsKey(E_ThrowType.Antitank))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Smoke];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Smoke].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                    else if (throwWeapon.throws2.ContainsKey(E_ThrowType.Smoke))
                    {
                        throwWeapon.throwObj = throwWeapon.throws2[E_ThrowType.Flash];
                        throwWeapon.grenade = throwWeapon.throws2[E_ThrowType.Flash].GetComponent<ThrowBullet>();
                        //可能播放切换手雷动画

                    }
                }


            }

        }


        
    }

    /// <summary>
    /// 找到自身武器控件并存储到武器字典中,初始化武器字典
    /// 初始状态下玩家各种武器状态一定是激活状态
    /// </summary>
    /// <typeparam name="T">继承BaseWeapon的类</typeparam>
    private void FindWeapon<T>() where T : BaseWeapon
    {
        T[] controls = this.GetComponentsInChildren<T>();
        for (int i = 0; i < controls.Length; i++)
        {
            if (controls[i] is PrimaryWeapon)
            {
                WeaponDic.Add(E_WeaponType.Primary, controls[i].gameObject);
                controls[i].gameObject.SetActive(true);
                controls[i].GetComponent<PrimaryWeapon>().SetFather(this);
                currentPlayerWeapon = controls[i].gameObject;
                currentWeapon = controls[i].GetComponent<PrimaryWeapon>();
            }
            if (controls[i] is SecondaryWeapon)
            {
                WeaponDic.Add(E_WeaponType.Secondary, controls[i].gameObject);
                controls[i].gameObject.SetActive(false);
                controls[i].GetComponent<SecondaryWeapon>().SetFather(this);
            }
            if (controls[i] is CloseWeapon)
            {
                WeaponDic.Add(E_WeaponType.Close, controls[i].gameObject);
                controls[i].gameObject.SetActive(false);
                //currentPlayerWeapon = controls[i].gameObject;
                //currentWeapon = controls[i].GetComponent<CloseWeapon>();
                controls[i].GetComponent<CloseWeapon>().SetFather(this);
            }
            if (controls[i] is ThrowWeapon)
            {
                WeaponDic.Add(E_WeaponType.Throw, controls[i].gameObject);
                controls[i].gameObject.SetActive(false);
                controls[i].GetComponent<ThrowWeapon>().SetFather(this);
            }
        }
        Debug.Log(WeaponDic.Count);
    }
    #endregion

    #region 受伤逻辑
    /// <summary>
    /// 受伤逻辑
    /// </summary>
    /// <param name="other"></param>
    public override void Wound(PlayerBaseObj other)
    {
        base.Wound(other);
    }
    #endregion

    #region 玩家死亡逻辑
    /// <summary>
    /// 玩家死亡逻辑
    /// </summary>
    public override void Dead()
    {
        base.Dead();
    }
    #endregion
}
