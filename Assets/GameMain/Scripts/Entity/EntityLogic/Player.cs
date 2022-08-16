using GameFramework;
using GamePlayer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    public enum CtrlType
    {
        player,
        net,
    }

    /// <summary>
    /// 玩家类
    /// </summary>
    public class Player : Entity
    {
        public PlayerData _PlayerData { get { return m_PlayerData; } }

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
        public float mouseSensitivity;
        public Texture2D tex;
        public Transform m_playerCamera;
        public float xRotation = 0f;

        //角色状态
        public PlayerStats playerStats;

        float mouseX;
        float mouseY;

        private CharacterController m_Controller;

        public Animator ThridPersonAnimator;

        private Transform m_AimTarget;

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
            m_Controller = GetComponent<CharacterController>();
            mouseSensitivity = GameEntry.Setting.GetInt("MouseSensitivity");
            groundCheck = transform.Find("GroundCheck");
            m_playerCamera = transform.Find("WorldCamera");
            groundMask = LayerMask.GetMask("Ground");

            ThridPersonAnimator = GetComponentInChildren<Animator>();
            m_AimTarget = transform.Find("AimTarget");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("Player data is invalid.");
            }

            Name = Utility.Text.Format("Player ({0})", m_PlayerData.Name);

            List<GunData> gunDatas = m_PlayerData.GetAllGunDatas();
            for (int i = 0; i < gunDatas.Count; i++)
            {
                GameEntry.Entity.ShowGun(gunDatas[i]);
            }

            GameEntry.Event.Fire(this, PlayerOnShowEventArgs.Create(this.m_PlayerData.Id));

            transform.position = m_PlayerData.Position;
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            if (childEntity is Gun)
            {
                m_Guns.Add((Gun)childEntity);
                return;
            }
            if (childEntity is MeleeWeapon)
            {
                m_MeleeWeapons.Add((MeleeWeapon)childEntity);
                return;
            }
            if (childEntity is Thrown)
            {
                m_Throwns.Add((Thrown)childEntity);
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);


        }

        private void FixedUpdate()
        {
            //每帧进行重力检测
            GravitySimulation();
            if (m_PlayerData.CtrlType == CtrlType.net)
            {
                NetUpdate();
                //return;
            }
            PlayerCtrl();
        }

        private float lastSendInfoTime = float.MinValue;

        //玩家控制
        public void PlayerCtrl()
        {
            if (m_PlayerData.CtrlType != CtrlType.player)
                return;

            //每帧检测玩家的移动
            CheckBodyAxis(new string[2] { "Horizontal", "Vertical" });
            //每帧检查玩家的鼠标移动
            CheckMouseAxis(new string[2] { "Mouse X", "Mouse Y" });
            //每帧检查玩家是否跳跃按下
            CheckButtonDown("Jump");
            //每帧检测玩家是否按下下蹲
            CheckKeyCode(KeyCode.LeftControl);
            //检测换枪
            CheckKeyCode(KeyCode.Alpha1);
            CheckKeyCode(KeyCode.Alpha2);

            if (m_CurrentGun != null)
            {
                //每帧检测玩家是否开火
                CheckMouseButtonDown(0);
                //每帧检测玩家是否开镜
                CheckMouseButtonDown(1);
                //每帧检测玩家是否换弹
                CheckKeyCode(KeyCode.R);
            }

            //网络同步
            if (Time.time - lastSendInfoTime > 0.1f)
            {
                SendMoveInfo();
                lastSendInfoTime = Time.time;
            }

        }
        //发送位置信息
        private void SendMoveInfo()
        {
            MoveMsg msg = new MoveMsg();
            PlayerPos pos = new PlayerPos();
            pos.id = GameEntry.Net.ID;
            pos.posX = transform.position.x;
            pos.posY = transform.position.y;
            pos.posZ = transform.position.z;
            pos.rotX = transform.eulerAngles.x;
            pos.rotY = transform.eulerAngles.y;
            pos.rotZ = transform.eulerAngles.z;
            msg.playerPos = pos;
            GameEntry.Net.Send(msg);
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

        }

        /// <summary>
        /// 玩家按键触发
        /// </summary>
        /// <param name="key"></param>
        private void CheckKeyCode(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.R:
                    if (Input.GetKeyDown(key))
                    {
                        m_CurrentGun.ReloadBullet();
                    }
                    break;
                case KeyCode.LeftControl:
                    Crouch(key);
                    break;
                case KeyCode.Alpha1:
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        WeaponSwap(0);
                        ThridPersonAnimator.CrossFade("Rifle Aim", 0.2f);
                        GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha1));
                    }
                    break;
                case KeyCode.Alpha2:
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        WeaponSwap(1);
                        ThridPersonAnimator.CrossFade("Pistol Aim", 0.2f);
                        GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha2));
                    }
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
                    if (cFireTime >= m_CurrentGun.AttackInterval)
                    {
                        cFireTime = 0;
                        Vector3 vector = m_CurrentGun.excursion.Dequeue();
                        Debug.Log(vector);
                        xRotation += vector.x;
                        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

                        m_playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                        transform.Rotate(Vector3.up * vector.y);
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
        /// 玩家切枪逻辑
        /// </summary>
        public void WeaponSwap(int index)
        {
            if (m_CurrentGun != null)
            {
                m_AimTarget.SetParent(transform);
                m_CurrentGun.FirstPersonAnimator.SetTrigger("Unwield");
                StartCoroutine(RealWeaponSwap(index));
            }
            else
            {
                m_Guns[index].gameObject.SetActive(true);
                m_CurrentGun = m_Guns[index];
                m_AimTarget.SetParent(m_CurrentGun.ShootPoint);
                m_AimTarget.localPosition = Vector3.zero;
                m_AimTarget.position += m_CurrentGun.ShootPoint.forward * 2f;
            }
        }

        /// <summary>
        /// 协程换枪
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private IEnumerator RealWeaponSwap(int index)
        {
            yield return null;
            AnimatorStateInfo stateinfo = m_CurrentGun.FirstPersonAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateinfo.IsName("Unwield") && (stateinfo.normalizedTime >= 1.0f))
            {
                m_CurrentGun.gameObject.SetActive(false);

                m_Guns[index].gameObject.SetActive(true);
                m_CurrentGun = m_Guns[index];
                m_AimTarget.SetParent(m_CurrentGun.ShootPoint);
                m_AimTarget.localPosition = Vector3.zero;
                m_AimTarget.position += m_CurrentGun.ShootPoint.forward * 2f;
            }
            else
            {
                StartCoroutine(RealWeaponSwap(index));
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

            float nX = Input.GetAxisRaw(axis[0]);
            float nZ = Input.GetAxisRaw(axis[1]);

            Vector3 move = transform.right * nX + transform.forward * nZ;
            m_Controller.Move(move * m_PlayerData.Speed * Time.deltaTime);
            ThridPersonAnimator.SetFloat("VelocityX", x);
            ThridPersonAnimator.SetFloat("VelocityZ", z);

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
            xRotation = Mathf.Clamp(xRotation, -80f, 40f);

            m_playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
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
                            }
                            ThridPersonAnimator.SetTrigger("Jump");
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
            m_Controller.Move(velocity * Time.deltaTime);
        }

        /// <summary>
        /// 按下 LeftControl 进行下蹲
        /// </summary>
        private void Crouch(KeyCode key)
        {
            if (Input.GetKeyDown(key))
            {
                //播放下蹲动画，降低移动速度，摄像头高度下降
                ThridPersonAnimator.CrossFade("Stand To Crouch", 0.2f);
            }
            if (Input.GetKeyUp(key))
            {
                //LeftControl抬起取消下蹲动画，并且回复速度
                ThridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
            }
        }


        #region CtrlNet

        //last 上次的位置信息
        Vector3 lPos;
        Vector3 lRot;
        //forecast 预测的位置信息
        Vector3 fPos;
        Vector3 fRot;

        float speedMultiple = 1f;

        //位置预测
        public void NetForecastInfo(Vector3 nPos, Vector3 nRot)
        {
            //预测的位置
            fPos = lPos + (nPos - lPos) * 1.2f;
            fRot = lRot + (nRot - lRot) * 1.2f;
            lPos = nPos;
            lRot = nRot;
        }

        //初始化位置预测数据
        public void InitNetCtrl()
        {
            m_playerCamera.gameObject.SetActive(false);
            UnityUtility.ChangeLayer(transform, "ThridPerson_Other");

            lPos = transform.position;
            lRot = transform.eulerAngles;
            fPos = transform.position;
            fRot = transform.eulerAngles;

        }

        public void NetUpdate()
        {
            //当前位置
            Vector3 pos = transform.position;
            Vector3 rot = transform.eulerAngles;

            //更新位置
            var offset = (fPos - transform.position);
            if (offset.sqrMagnitude > 2f)
            {
                //playerBody.position = fPos;
                //playerBody.eulerAngles = fRot;
                speedMultiple = 1.2f;
            }

            //移动和转向
            if (offset.sqrMagnitude > 0.005f)
            {
                offset = offset.normalized;
                ThridPersonAnimator.SetFloat("VelocityX", offset.x, Time.deltaTime * 5, Time.deltaTime);
                ThridPersonAnimator.SetFloat("VelocityZ", offset.z, Time.deltaTime * 5, Time.deltaTime);
                m_Controller.Move(offset * m_PlayerData.Speed * speedMultiple * Time.deltaTime);
            }
            else
            {
                speedMultiple = 1f;
                ThridPersonAnimator.SetFloat("VelocityX", 0, Time.deltaTime * 5, Time.deltaTime);
                ThridPersonAnimator.SetFloat("VelocityZ", 0, Time.deltaTime * 5, Time.deltaTime);
            }
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot),
                                              Quaternion.Euler(fRot), Time.deltaTime * 10);


            //跳跃
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
            if (!isGrounded)
                ThridPersonAnimator.SetTrigger("Jump");


        }

        #endregion
    }
}