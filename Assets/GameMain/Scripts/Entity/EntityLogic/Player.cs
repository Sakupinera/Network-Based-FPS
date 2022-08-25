using GameFramework;
using GameFramework.Event;
using GamePlayer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public PlayerData GetPlayerData { get { return m_PlayerData; } }

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

        //public int killNum = 0;

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
        public E_PLAYER_STATUS playerStatus = E_PLAYER_STATUS.Normal;

        float mouseX;
        float mouseY;

        private CharacterController m_Controller;

        public Animator FirstPersonAnimator;
        public Animator ThridPersonAnimator;

        private Transform m_AimTarget;

        private static readonly int HashAimingAlpha = Animator.StringToHash("Aiming");

        private static readonly int HashMovement = Animator.StringToHash("Movement");

        private bool aiming;

        //按下开火键的累计时间
        public float cFireTime = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);


            //获得自身控件
            m_Controller = GetComponent<CharacterController>();
            mouseSensitivity = GameEntry.Setting.GetFloat("MouseSensitivity");
            groundCheck = transform.Find("GroundCheck");
            m_playerCamera = transform.Find("View");
            groundMask = LayerMask.GetMask("Ground");

            FirstPersonAnimator = transform.Find("View/Arms_FirstPerson/SK_FP_CH_Default_Root").GetComponent<Animator>();
            ThridPersonAnimator = GetComponentInChildren<Animator>();

            m_AimTarget = transform.Find("AimTarget");

            GameEntry.Event.Subscribe(ChangeMouseSensitivityEnventArgs.EventId, OnMouseSensitivityChanged);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);


            //鼠标设定相关
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);

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

        public void InitPLayerCtrl()
        {
            UnityUtility.ChangeLayer(transform.Find("View/Arms_FirstPerson"), "FirstPerson");
            m_CurrentGun = null;
            m_AimTarget.SetParent(transform);
            m_AimTarget.position = transform.forward * 2;
            FirstPersonAnimator.SetBool("Holstered", true);
            StartCoroutine(RealRetractWeapon());
            GameEntry.UI.CloseUIForm((GameEntry.Procedure.CurrentProcedure as ProcedureBattle).loadingFormID);
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

            //生命值为0死亡
            if (m_PlayerData.HP <= 0 && playerStatus != E_PLAYER_STATUS.Die)
            {
                Debug.LogWarning("AWSL");
                RetractWeapon();
                GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha3, Id));
                Dead();
            }

        }

        private void FixedUpdate()
        {
            if (playerStatus == E_PLAYER_STATUS.Die)
                return;

            if (m_PlayerData.CtrlType == CtrlType.net)
            {
                NetUpdate();
                return;
            }

            PlayerCtrl();
        }

        public void OnMouseSensitivityChanged(object sender, GameEventArgs e)
        {
            ChangeMouseSensitivityEnventArgs ne = (ChangeMouseSensitivityEnventArgs)e;
            mouseSensitivity = ne.MouseSensitivity;
        }

        private float lastSendInfoTime = float.MinValue;

        //玩家控制
        public void PlayerCtrl()
        {
            if (m_PlayerData.CtrlType != CtrlType.player)
                return;

            //每帧进行重力检测
            GravitySimulation();
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
            //CheckKeyCode(KeyCode.Alpha3);
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
        //发送位置信息（update发送）
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
            pos.tPosX = m_AimTarget.position.x;
            pos.tPosY = m_AimTarget.position.y;
            pos.tPosZ = m_AimTarget.position.z;
            msg.playerPos = pos;
            GameEntry.Net.Send(msg);
        }

        //发送状态消息（切换状态发送）
        private void SendStatusInfo()
        {
            StatusMsg msg = new StatusMsg();
            msg.playerStatus = new PlayerStatus();
            msg.playerStatus.id = GameEntry.Net.ID;
            msg.playerStatus.playerStatus = playerStatus;
            GameEntry.Net.Send(msg);
        }

        //发送武器信息（切换武器或换弹发送）
        public void SendWeaponInfo(int weaponID, bool isReload)
        {
            WeaponMsg msg = new WeaponMsg();
            msg.id = GameEntry.Net.ID;
            msg.weaponID = weaponID;
            msg.isReload = isReload;
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
                    Aim(i);
                    break;
            }
        }

        [SerializeField]
        private float dampTimeAiming = 0.3f;

        /// <summary>
        /// 开镜逻辑
        /// </summary>
        public void Aim(int i)
        {
            if (Input.GetMouseButtonDown(i))
            {
                aiming = true;
            }
            if (Input.GetMouseButtonUp(i))
            {
                aiming = false;
            }

            FirstPersonAnimator.SetFloat(HashAimingAlpha, Convert.ToSingle(aiming), 0.25f / 1.0f * dampTimeAiming, Time.deltaTime);

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
                        //ThridPersonAnimator.z;
                        Debug.Log(m_PlayerData.Name + "换枪 步枪");
                        ThridPersonAnimator.CrossFade("Rifle Aim", 0.2f);
                        GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha1, Id));
                    }
                    break;
                case KeyCode.Alpha2:
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        WeaponSwap(1);
                        Debug.Log(m_PlayerData.Name + "换枪 手枪");
                        ThridPersonAnimator.CrossFade("Pistol Aim", 0.2f);
                        GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha2, Id));
                    }
                    break;
                case KeyCode.Alpha3:
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        RetractWeapon();
                        Debug.Log("收枪");
                        GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha3, Id));
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
                if (m_CurrentGun.currentMagBullets > 0 && m_CurrentGun.reloadTimer >= m_CurrentGun.ReloadRate)
                {
                    cFireTime += Time.deltaTime;
                    if (cFireTime >= m_CurrentGun.AttackInterval)
                    {
                        cFireTime = 0;
                        Vector3 vector = m_CurrentGun.excursion.Dequeue();
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
        /// 收枪
        /// </summary>
        private void RetractWeapon()
        {
            if (m_CurrentGun != null)
            {
                m_CurrentGun = null;
                m_AimTarget.SetParent(transform);
                m_AimTarget.position = transform.forward * 2;
                FirstPersonAnimator.SetBool("Holstered", true);
                StartCoroutine(RealRetractWeapon());
            }
            else
            {
                return;
            }
        }

        private IEnumerator RealRetractWeapon()
        {
            yield return null;
            AnimatorStateInfo stateinfo = m_CurrentGun.GunAnimator.GetCurrentAnimatorStateInfo(0);
            if ((stateinfo.normalizedTime >= 1.0f))
            {
                ThridPersonAnimator.CrossFade("Unarmed Locomotion", 0.2f);
                foreach (var e in m_Guns)
                {
                    e.gameObject.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(RealRetractWeapon());
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
                //m_CurrentGun.GunAnimator.SetTrigger("Unwield");
                FirstPersonAnimator.SetBool("Holstered", true);
                StartCoroutine(RealWeaponSwap(index));
            }
            else
            {
                m_Guns[index].gameObject.SetActive(true);
                FirstPersonAnimator.runtimeAnimatorController = m_Guns[index].GetComponent<FirstPersonAnimationController>().AnimatorController;
                m_CurrentGun = m_Guns[index];

                m_AimTarget.SetParent(m_CurrentGun.ShootPoint);
                m_AimTarget.localPosition = Vector3.zero;
                m_AimTarget.position += m_CurrentGun.ShootPoint.forward * 2f;

                SendWeaponInfo(index, false);
            }
        }

        /// <summary>
        /// 协程换枪
        /// </summary>
        /// <param name="index">0是第1把枪，1是第2把枪</param>
        /// <returns></returns>
        private IEnumerator RealWeaponSwap(int index)
        {
            yield return null;
            AnimatorStateInfo stateinfo = FirstPersonAnimator.GetCurrentAnimatorStateInfo(0);
            //Debug.LogWarning(stateinfo.normalizedTime);
            if (/*stateinfo.IsName("Holster") &&*/(stateinfo.normalizedTime >= 1.0f))
            {
                // 第一人称的换枪
                m_CurrentGun.gameObject.SetActive(false);
                m_Guns[index].gameObject.SetActive(true);
                FirstPersonAnimator.runtimeAnimatorController = m_Guns[index].GetComponent<FirstPersonAnimationController>().AnimatorController;
                m_CurrentGun = m_Guns[index];
                FirstPersonAnimator.SetBool("Holstered", false);

                // 第三人称Animation Rigging的瞄准目标
                m_AimTarget.SetParent(m_CurrentGun.ShootPoint);
                m_AimTarget.localPosition = Vector3.zero;
                m_AimTarget.position += m_CurrentGun.ShootPoint.forward * 2f;

                SendWeaponInfo(index, false);
            }
            else
            {
                StartCoroutine(RealWeaponSwap(index));
            }

        }

        [SerializeField]
        private float dampTimeLocomotion = 0.15f;

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

            FirstPersonAnimator.SetFloat(HashMovement, Mathf.Clamp01(Mathf.Abs(x) + Mathf.Abs(z)), dampTimeLocomotion, Time.deltaTime);

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
                                ThridPersonAnimator.SetTrigger("Jump");
                            }
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
                if (playerStatus != E_PLAYER_STATUS.Crouch)
                {
                    playerStatus = E_PLAYER_STATUS.Crouch;
                    SendStatusInfo();
                }
            }
            if (Input.GetKeyUp(key))
            {
                //LeftControl抬起取消下蹲动画，并且回复速度
                ThridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
                if (playerStatus != E_PLAYER_STATUS.Normal)
                {
                    playerStatus = E_PLAYER_STATUS.Normal;
                    SendStatusInfo();
                    //print("站起");
                }
            }
        }

        public bool isSuicide = false;
        //角色死亡
        public void Dead()
        {
            //更新并发送状态消息
            playerStatus = E_PLAYER_STATUS.Die;

            ThridPersonAnimator.SetTrigger("Die");

            SendStatusInfo();
            if (isSuicide == false)
                StartCoroutine(Revive());
            else
                HideMyself();
        }


        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            m_Guns.Clear();
            m_CurrentGun = null;

        }

        public void HideMyself()
        {
            GameEntry.Entity.HideEntity(this.Id);
        }

        //复活
        private IEnumerator Revive()
        {
            //3秒后复活
            yield return new WaitForSeconds(3);
            print("复活玩家 Name：" + m_PlayerData.Name);
            Transform sp = GameObject.Find("SwopPoints").transform;
            Transform swopTrans;
            if (m_PlayerData.Camp == CampType.BlueCamp)
            {
                swopTrans = sp.GetChild(0);
            }
            else
            {
                swopTrans = sp.GetChild(1);
            }
            //更新并发送状态消息
            playerStatus = E_PLAYER_STATUS.Normal;
            m_PlayerData.HP = 100;
            if (m_PlayerData.CtrlType == CtrlType.player)
            {
                GameEntry.Event.Fire(this, PlayerOnHPChangedEventArgs.Create(100));
            }
            SendStatusInfo();
            //更新位置
            transform.position = swopTrans.position;
            //刷新武器
            ResetWeaponDatas();
        }

        private void ResetWeaponDatas()
        {
            foreach (var e in m_Guns)
            {
                e.ResetGunData();
            }
        }

        #region CtrlNet

        //last 上次的位置信息
        Vector3 lPos;
        Vector3 lRot;
        Vector3 ltPos;
        //forecast 预测的位置信息
        Vector3 fPos;
        Vector3 fRot;
        Vector3 ftPos;

        E_PLAYER_STATUS lplayerStatus;

        //int lweaponID = -1;

        float speedMultiple = 1f;

        float speedStatus = 1f;

        //Net玩家武器信息
        public void NetWeapon(int weaponID, bool isReload)
        {
            //换弹
            if (isReload)
            {
                Debug.LogWarning("Net玩家" + m_PlayerData.Name + "  换弹");
                ThridPersonAnimator.SetTrigger("Reload");
                return;
            }
            Debug.LogWarning("Net玩家" + m_PlayerData.Name + "  武器：" + weaponID);
            if (weaponID == 0)
            {
                Debug.Log(m_PlayerData.Name + " " + Id + " " + "换枪 步枪");
                GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha1, Id));
                ThridPersonAnimator.CrossFade("Rifle Aim", 0.2f);
            }
            else if (weaponID == 1)
            {
                Debug.Log(m_PlayerData.Name + " " + Id + " " + "换枪 手枪");
                GameEntry.Event.Fire(this, SwapWeaponSuccessEventArgs.Create(KeyCode.Alpha2, Id));
                ThridPersonAnimator.CrossFade("Pistol Aim", 0.2f);
            }

            //}
        }

        //Net玩家切换状态
        public void NetChangeStatus()
        {
            if (playerStatus == E_PLAYER_STATUS.Normal)
            {
                if (lplayerStatus != E_PLAYER_STATUS.Normal)
                {
                    ThridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
                    lplayerStatus = E_PLAYER_STATUS.Normal;
                }
                speedStatus = 1f;
            }
            else if (playerStatus == E_PLAYER_STATUS.Crouch)
            {
                if (lplayerStatus != E_PLAYER_STATUS.Crouch)
                {
                    ThridPersonAnimator.CrossFade("Stand To Crouch", 0.2f);
                    lplayerStatus = E_PLAYER_STATUS.Crouch;
                }
                speedStatus = 0.5f;
            }
            else if (playerStatus == E_PLAYER_STATUS.Silent)
            {
                speedStatus = 0.8f;
            }
            else if (playerStatus == E_PLAYER_STATUS.Die)
            {
                Dead();
            }
        }

        //位置预测
        public void NetForecastInfo(Vector3 nPos, Vector3 nRot, Vector3 ntPos)
        {
            //预测的位置
            fPos = lPos + (nPos - lPos) * 1.2f;
            fRot = lRot + (nRot - lRot) * 1.2f;
            ftPos = ltPos + (ntPos - ltPos) * 1.2f;
            lPos = nPos;
            lRot = nRot;
            ltPos = ntPos;

        }

        //初始化位置预测数据
        public void InitNetCtrl()
        {
            foreach (var e in m_playerCamera.GetComponentsInChildren<Camera>())
            {
                e.enabled = false;
            }
            UnityUtility.ChangeLayer(transform, "ThridPerson_Other");
            UnityUtility.ChangeLayer(transform.Find("View/Arms_FirstPerson"), "CantSee");

            lPos = transform.position;
            lRot = transform.eulerAngles;
            fPos = transform.position;
            fRot = transform.eulerAngles;

        }
        public void NetUpdate()
        {

            NetChangeStatus();

            //当前位置
            Vector3 pos = transform.position;
            Vector3 rot = transform.eulerAngles;
            Vector3 tPos = m_AimTarget.position;

            //更新位置
            var offset = (fPos - transform.position);
            if (offset.sqrMagnitude > 2f)
            {
                speedMultiple = 1.2f;
            }
            else if (offset.sqrMagnitude > 10f)
            {
                transform.position = fPos;
                transform.eulerAngles = fRot;
            }


            //移动、转向、朝向
            if (offset.sqrMagnitude > 0.005f)
            {
                offset = offset.normalized;
                ThridPersonAnimator.SetFloat("VelocityX", offset.x, Time.deltaTime * 5, Time.deltaTime);
                ThridPersonAnimator.SetFloat("VelocityZ", offset.z, Time.deltaTime * 5, Time.deltaTime);
                m_Controller.Move(offset * m_PlayerData.Speed * speedMultiple * speedStatus * Time.deltaTime);
            }
            else
            {
                speedMultiple = 1f;
                ThridPersonAnimator.SetFloat("VelocityX", 0, Time.deltaTime * 5, Time.deltaTime);
                ThridPersonAnimator.SetFloat("VelocityZ", 0, Time.deltaTime * 5, Time.deltaTime);
            }
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(rot),
                                              Quaternion.Euler(fRot), Time.deltaTime * 10);
            //朝向
            m_AimTarget.position = Vector3.Lerp(tPos, ftPos, Time.deltaTime * 30);

            //跳跃
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
            if (!isGrounded)
            {
                ThridPersonAnimator.SetTrigger("Jump");
            }
        }


        #endregion

    }
}