using GameFramework.DataTable;
using GameFramework.Event;
using GameServer;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        private void StartGame(object sender, GameEventArgs e)
        {
            GameEntry.UI.CloseAllLoadedUIForms();
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(MsgEventArgs<ResponseFightMsg>.EventId, StartGame);

            m_StartGame = false;

            // 加载主菜单UIForm
            GameEntry.UI.OpenUIForm(UIFormId.TestForm);

        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);


            GameEntry.Event.Unsubscribe(MsgEventArgs<ResponseFightMsg>.EventId, StartGame);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame == true)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Battle"));
                //procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Team);
                procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Single);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

    }
}
