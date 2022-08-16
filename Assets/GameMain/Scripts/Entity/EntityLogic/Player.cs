using GameFramework;
using GamePlayer;
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
        public Transform playerBody;
        public Transform playerCame;
        public float xRotation = 0f;

        //角色状态
        //public PlayerStats playerStats;
        public E_PLAYER_STATUS playerStatus = E_PLAYER_STATUS.Normal;

        float mouseX;
        float mouseY;

        public CharacterController controller;

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
            mouseSensitivity = GameEntry.Setting.GetInt("MouseSensitivity");
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

            //生命值为0死亡
            if (m_PlayerData.HP <= 0)
            {
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
            //pos.tPosX
            //pos.tPosY
            //pos.tPosZ
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
        private void SendWeaponInfo(int weaponID, bool isReload)
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
            switch (key)
            {
                case KeyCode.R:
                    if (Input.GetKeyDown(key))
                        m_CurrentGun.ReloadBullet();
                    break;
                case KeyCode.LeftControl:
                    Crouch(key);
                    break;
                case KeyCode.Alpha1:
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                        WeaponSwap(0);
                    break;
                case KeyCode.Alpha2:
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                        WeaponSwap(1);
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
        /// 玩家切枪逻辑
        /// </summary>
        public void WeaponSwap(int index)
        {
            if (m_CurrentGun != null)
            {
                m_CurrentGun.FirstPersonAnimator.SetTrigger("Unwield");
                StartCoroutine(RealWeaponSwap(index));
            }
            else
            {
                m_Guns[index].gameObject.SetActive(true);
                m_CurrentGun = m_Guns[index];
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
                                thridPersonAnimator.SetTrigger("Jump");
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
                if (playerStatus != E_PLAYER_STATUS.Crouch)
                {
                    playerStatus = E_PLAYER_STATUS.Crouch;
                    SendStatusInfo();
                    //print("蹲下");
                }
            }
            if (Input.GetKeyUp(key))
            {
                //LeftControl抬起取消下蹲动画，并且回复速度
                thridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
                if (playerStatus != E_PLAYER_STATUS.Normal)
                {
                    playerStatus = E_PLAYER_STATUS.Normal;
                    SendStatusInfo();
                    //print("站起");
                }
            }
        }

        //角色死亡
        public void Dead()
        {
            //更新并发送状态消息
            playerStatus = E_PLAYER_STATUS.Die;
            SendStatusInfo();
            StartCoroutine(Revive());
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
            SendStatusInfo();
            //更新位置
            transform.position = swopTrans.position;
            //刷新武器
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

        int lweaponID;

        float speedMultiple = 1f;

        float speedStatus = 1f;

        //Net玩家武器信息
        public void NetWeapon(int weaponID, bool isReload)
        {
            //切换武器
            if (weaponID != lweaponID)
            {
                lweaponID = weaponID;

            }
            //换弹
            if (isReload)
            {

            }
        }

        //Net玩家切换状态
        public void NetChangeStatus()
        {
            if (playerStatus == E_PLAYER_STATUS.Normal)
            {
                if (lplayerStatus != E_PLAYER_STATUS.Normal)
                {
                    thridPersonAnimator.CrossFade("Crouch To Stand", 0.2f);
                    lplayerStatus = E_PLAYER_STATUS.Normal;
                }
                speedStatus = 1f;
            }
            else if (playerStatus == E_PLAYER_STATUS.Crouch)
            {
                if (lplayerStatus != E_PLAYER_STATUS.Crouch)
                {
                    thridPersonAnimator.CrossFade("Stand To Crouch", 0.2f);
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
            playerCame.gameObject.SetActive(false);
            ChangeLayer(transform, "ThridPerson_Other");

            lPos = transform.position;
            lRot = transform.eulerAngles;
            fPos = transform.position;
            fRot = transform.eulerAngles;

        }
        public void NetUpdate()
        {

            NetChangeStatus();

            //当前位置
            Vector3 pos = playerBody.position;
            Vector3 rot = playerBody.eulerAngles;

            //更新位置
            var offset = (fPos - transform.position);
            if (offset.sqrMagnitude > 2f)
            {
                speedMultiple = 1.2f;
            }
            else if (offset.sqrMagnitude > 10f)
            {
                playerBody.position = fPos;
                playerBody.eulerAngles = fRot;
            }


            //移动、转向、朝向
            if (offset.sqrMagnitude > 0.005f)
            {
                offset = offset.normalized;
                thridPersonAnimator.SetFloat("VelocityX", offset.x, Time.deltaTime * 5, Time.deltaTime);
                thridPersonAnimator.SetFloat("VelocityZ", offset.z, Time.deltaTime * 5, Time.deltaTime);
                controller.Move(offset * m_PlayerData.Speed * speedMultiple * speedStatus * Time.deltaTime);
            }
            else
            {
                speedMultiple = 1f;
                thridPersonAnimator.SetFloat("VelocityX", 0, Time.deltaTime * 5, Time.deltaTime);
                thridPersonAnimator.SetFloat("VelocityZ", 0, Time.deltaTime * 5, Time.deltaTime);
            }
            playerBody.rotation = Quaternion.Lerp(Quaternion.Euler(rot),
                                              Quaternion.Euler(fRot), Time.deltaTime * 10);
            //朝向


            //跳跃
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistence, groundMask);
            //if (isGrounded)
            //    isJumping = false;
            if (!isGrounded/* && isJumping == false*/)
            {
                print(m_PlayerData.Name + " 起跳");
                thridPersonAnimator.SetTrigger("Jump");
                //isJumping = true;
            }
        }

        private void ChangeLayer(Transform trans, string targetLayer)
        {
            if (LayerMask.NameToLayer(targetLayer) == -1)
            {
                Debug.Log("Layer中不存在,请手动添加LayerName");
                return;
            }
            //遍历更改所有子物体layer
            trans.gameObject.layer = LayerMask.NameToLayer(targetLayer);
            foreach (Transform child in trans)
            {
                ChangeLayer(child, targetLayer);
            }
        }
        #endregion
    }
}