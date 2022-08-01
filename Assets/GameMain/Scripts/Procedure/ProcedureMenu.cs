using GameFramework.DataTable;
using GameFramework.Event;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        private TestForm m_MenuForm = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("进入菜单流程，可以在这里加载菜单UI");

            // 订阅UI加载成功事件
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_StartGame = false;

            // 加载UI
            GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/TestForm.prefab", "Default", this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_MenuForm != null)
            {
                m_MenuForm.Close(isShutdown);
                m_MenuForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if(m_StartGame == true)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Battle"));
                procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Team);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }

        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            // 判断userData是否为自己
            if(ne.UserData != this)
            {
                return;
            }

            Log.Debug("UI_Menu成功加载！");
            m_MenuForm = (TestForm)ne.UIForm.Logic;
        }

    }
}
