using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityGameFramework.Runtime;

namespace NetworkBasedFPS {
    /// <summary>
    /// 玩家类
    /// </summary>
    public class Player : Entity
    {
        [SerializeField]
        private PlayerData m_PlayerData = null;

        [SerializeField]
        private List<Gun> m_Guns = new List<Gun>();

        [SerializeField]
        private Gun m_CurrentGun = null;

        [SerializeField]
        private List<MeleeWeapon> m_MeleeWeapons = new List<MeleeWeapon>();

        [SerializeField]
        private List<Thrown> m_Throwns = new List<Thrown>();

        //重力
        public float gravity = -9.81f;

        //一组用于进行地面判断的变量
        public Transform groundCheck;
        public float groundDistence = 0.4f;
        public LayerMask groundMask;

        //跳跃时所能达到的向上速度
        public float jumpHeight = 0.8f;

        public Vector3 velocity;

        //判定是否在地面
        public bool isGrounded;

        //用于鼠标转动视角的一系列变量
        public float mouseSensitivity = 100f;
        public Texture2D tex;
        public Transform playerBody;
        public Transform playerCame;
        public float xRotation = 0f;

        //移动音效
        public AudioSource walkAS;

        //角色状态
        public PlayerStats playerStats;

        float mouseX;
        float mouseY;

        public CharacterController controller;

        public Animator firstPersonAnimator;
        public Animator thridPersonAnimator;

        //按下开火键的累计时间
        public float cFireTime = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            //鼠标设定相关
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);

            //获得自身控件
            controller = GetComponent<CharacterController>();
            //walkAS = GetComponent<AudioSource>();
            groundCheck = transform.Find("GroundCheck");
            playerCame = transform.Find("WorldCamera");
            groundMask = LayerMask.GetMask("Ground");
            playerBody = this.transform;

            thridPersonAnimator = GetComponent<Animator>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = userData as PlayerData;
            if(m_PlayerData == null)
            {
                Log.Error("Player data is invalid.");
            }

            Name = Utility.Text.Format("Player ({0})", m_PlayerData.Name);

            List<GunData> gunDatas = m_PlayerData.GetAllGunDatas();
            for(int i = 0; i < gunDatas.Count; i++)
            {
                GameEntry.Entity.ShowGun(gunDatas[i]);
            }
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            if(childEntity is Gun)
            {
                m_Guns.Add((Gun)childEntity);
                m_CurrentGun = childEntity as Gun;
                return;
            }
            if(childEntity is MeleeWeapon)
            {
                m_MeleeWeapons.Add((MeleeWeapon)childEntity);
                return;
            }
            if(childEntity is Thrown)
            {
                m_Throwns.Add((Thrown)childEntity);
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //每帧检查玩家的鼠标移动
            CheckMouseAxis(new string[2] { "Mouse X", "Mouse Y" });
            //每帧检查玩家是否跳跃按下
            CheckButtonDown("Jump");
            //每帧检测玩家是否开火
            CheckMouseButtonDown(0);
            //每帧检测玩家是否按下下蹲
            CheckKeyCode(KeyCode.LeftControl);
            //每帧检测玩家的移动
            CheckBodyAxis(new string[2] { "Horizontal", "Vertical" });
            //每帧检测玩家是否换弹
            CheckKeyCode(KeyCode.R);
            //每帧检测玩家是否开镜
            CheckMouseButtonDown(1);

            //每帧进行重力检测
            GravitySimulation();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        /// <summary>
        /// 玩家按下鼠标触发
        /// </summary>
        /// <param name="i"></param>
        public void CheckMouseButtonDown(int i)
        {
            switch (i)
            {
                case 0:
                    Shoot();
                    break;
                case 1:
                    Aim();
                    break;
            }
        }

        /// <summary>
        /// 开镜逻辑
        /// </summary>
        public void Aim()
        {
            //if (Input.GetMouseButtonDown(1))
            //{
            //    if (m_CurrentGun != null)
            //    {

            //        StartCoroutine(AimTransform(0, m_CurrentGun.transform.localPosition, new Vector3(-0.083f, 0.098f, -0.115f)));
            //        //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(-0.923f, 3.337f, -0.954f)));
            //        m_CurrentGun.transform.localRotation = Quaternion.Euler(new Vector3(-0.923f, 3.337f, -0.954f));
            //    }
            //}
            //if (Input.GetMouseButtonUp(1))
            //{
            //    StartCoroutine(AimTransform(0, m_CurrentGun.transform.localPosition, new Vector3(-0.012f, 0.012f, 0f)));
            //    //StartCoroutine(AimTransform(1, playerWeapon.transform.localRotation.eulerAngles, new Vector3(0f, 0f, 0f)));
            //    m_CurrentGun.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            //}
        }

        //IEnumerator AimTransform(int i, Vector3 origin, Vector3 target)
        //{
        //    float timer = 0f;
        //    while (timer <= 1)
        //    {
        //        timer += Time.deltaTime * 5;
        //        if (i == 0)
        //        {
        //            m_CurrentGun.transform.localPosition = Vector3.Lerp(origin, target, timer);
        //        }
        //        else if (i == 1)
        //        {
        //            //playerWeapon.transform.localRotation = Quaternion.Euler(Vector3.Lerp(origin, target, timer));
        //        }

        //        yield return null;
        //    }
        //}

        /// <summary>
        /// 玩家按键触发
        /// </summary>
        /// <param name="key"></param>
        private void CheckKeyCode(KeyCode key)
        {
            switch(key){
                case KeyCode.R:
                    m_CurrentGun.ReloadBullet(key);
                    break;
                case KeyCode.LeftControl:
                    Crouch(key);
                    break;
            }
        }

        /// <summary>
        /// 开火方法
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Shoot()
        {
            if (Input.GetMouseButton(0))
            {
                //武器执行开火行为
                if (m_CurrentGun != null)
                {
                    m_CurrentGun.Fire();
                }
                //进行弹道偏移判断
                if (m_CurrentGun.currentBullects > 0 && m_CurrentGun.reloadTimer >= m_CurrentGun.ReloadRate)
                {
                    cFireTime += Time.deltaTime;
                    if (cFireTime >= m_CurrentGun.cFireMaxTime)
                    {
                        cFireTime = 0;
                        Vector3 vector = m_CurrentGun.excursion.Dequeue();
                        Debug.Log(vector);
                        xRotation += vector.x;
                        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                        playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                        playerBody.Rotate(Vector3.up * vector.y);
                        m_CurrentGun.excursion.Enqueue(vector);
                    }
                }
            }
            //鼠标抬起将累计时间清零
            if (Input.GetMouseButtonUp(0))
            {
                cFireTime = 0f;
            }
        }

        /// <summary>
        /// 玩家的移动水平逻辑
        /// </summary>
        /// <param name="axis"></param>
        private void CheckBodyAxis(string[] axis)
        {
            float[] num = new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1]) };

            float x = num[0];
            float z = num[1];

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * m_PlayerData.Speed * Time.deltaTime);
            thridPersonAnimator.SetFloat("VelocityX", x);
            thridPersonAnimator.SetFloat("VelocityZ", z);
            //后续做Crouch动作时，再额外加上蹲下静音的判断
            //在地面上 且有速度变化，播放走路音效
            //if (move.sqrMagnitude > 0.9f && isGrounded)
            //{
            //    //走路音效未播放
            //    if (!walkAS.isPlaying)
            //    {
            //        //播放
            //        walkAS.Play();
            //    }
            //}
            //else
            //{
            //    //走路音效播放中
            //    if (walkAS.isPlaying)
            //    {
            //        //暂停
            //        walkAS.Pause();
            //    }

            //}
        }

        /// <summary>
        /// 玩家的镜头逻辑
        /// </summary>
        /// <param name="axis"></param>
        private void CheckMouseAxis(string[] axis)
        {
            float[] nums = new float[2] { Input.GetAxis(axis[0]), Input.GetAxis(axis[1]) };

            mouseX = nums[0] * mouseSensitivity * Time.deltaTime;
            mouseY = nums[1] * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            playerCame.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        /// <summary>
        /// 玩家的按下Jump的逻辑
        /// </summary>
        /// <param name="button"></param>
        private void CheckButtonDown(string button)
        {
            if (Input.GetButtonDown(button))
            {
                switch (button)
                {
                    case "Jump":
                        {
                            //Physics.CheckSpher在一个指定位置创建一个指定半径的球体，并与指定的层级的物体进行碰撞判断
                            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
                            if (isGrounded)
                            {
                                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                                playerStats = PlayerStats.JUMP;
                            }
                            thridPersonAnimator.SetTrigger("Jump");
                        }
                        break;
                }
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
        /// 按下 LeftControl 进行下蹲
        /// </summary>
        private void Crouch(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                //播放下蹲动画，降低移动速度，摄像头高度下降
                thridPersonAnimator.CrossFade("Stand To Crouch", 0.2f);
            }
            if (Input.GetKeyUp(key))
            {
                //LeftControl抬起取消下蹲动画，并且回复速度
                thridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
            }
        }
    }
}