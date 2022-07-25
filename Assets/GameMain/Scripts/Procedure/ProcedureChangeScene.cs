using GameFramework.DataTable;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private const int MenuSceneId = 1;

        private bool m_ChangeToMenu = false;
        private bool m_IsChangeSceneComplete = false;
        private int m_BackgroundMusicId = 0;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            
        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            
        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            
        }

        private void OnLoadSceneUpdate(object sender, GameEventArgs e)
        {
            
        }

        private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
        {
            
        }
    }
}
