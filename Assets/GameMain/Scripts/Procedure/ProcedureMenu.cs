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
        private MenuForm m_MenuForm = null;

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

            // 加载UI
            GameEntry.UI.OpenUIForm("Assets/GameMain/UI/UIForms/TestForm.prefab", "DefaultGroup", this);

            // 订阅
            GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);

            // 加载配置表
            DataTableBase dataTableBase = GameEntry.DataTable.CreateDataTable(Type.GetType("DRHero"));
            dataTableBase.ReadData("Assets/MainGame/DataTables/Hero.txt");
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
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
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {
            // 获取框架数据表组件
            DataTableComponent DataTable
                = UnityGameFramework.Runtime.GameEntry.GetComponent<DataTableComponent>();
            // 获得数据表
            IDataTable<DRHero> dtScene = DataTable.GetDataTable<DRHero>();

            // 获得所有行
            DRHero[] drHeros = dtScene.GetAllDataRows();
            Log.Debug("drHeros:" + drHeros.Length);
            // 根据行号获得某一行
            DRHero drScene = dtScene.GetDataRow(1); // 或直接使用 dtScene[1]
            if (drScene != null)
            {
                // 此行存在，可以获取内容了
                string name = drScene.Name;
                int atk = drScene.Atk;
                Log.Debug("name:" + name + ", atk:" + atk);
            }
            else
            {
                // 此行不存在
            }

            // 获得满足条件的第一行
            DRHero drSceneWithCondition = dtScene.GetDataRow(x => x.Name == "Sakupinera");

        }
    }
}
