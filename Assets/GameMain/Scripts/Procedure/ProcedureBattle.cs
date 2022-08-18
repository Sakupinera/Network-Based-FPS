using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS
{
    public class ProcedureBattle : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 1f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public GameBase CurrentGame
        {
            get { return m_CurrentGame; }
        }



        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            m_Games.Add(GameMode.Team, new TeamGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;
            GameMode gameMode = (GameMode)procedureOwner.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();

            GameEntry.UI.OpenUIForm(UIFormId.GameForm);

            Log.Debug("开始战斗！");
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //  如果当前游戏不为空，并且游戏没有结束
            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Log.Debug("Escape Pressed");
                    GameEntry.UI.OpenUIForm(UIFormId.EscForm);
                    Cursor.lockState = CursorLockMode.None;
                }
                return;
            }

            //  自动结束游戏返回主菜单
            //if (!m_GotoMenu)
            //{
            //    m_GotoMenu = true;
            //    m_GotoMenuDelaySeconds = 0;
            //}

            //  经过xx秒返回主菜单
            //m_GotoMenuDelaySeconds += elapseSeconds;
            //if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            //{
            //    Debug.LogWarning("返回主菜单");
            //    procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
            //    ChangeState<ProcedureChangeScene>(procedureOwner);
            //}

            if(m_GotoMenu == true)
            {
                Debug.LogWarning("返回主菜单");
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                procedureOwner.SetData<VarBoolean>("isBattleToMenu", true);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }

        }
    }
}
