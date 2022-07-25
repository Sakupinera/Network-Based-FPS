using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS
{
    public class ProcedurePreload : ProcedureBase
    {
        public static readonly string[] DataTableNames = new string[]
        {
            "Scene",
            "Hero",
        };

        private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

        public override bool UseNativeDialog
        {
            get
            {
                return true;
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

        private void PreloadResources()
        {

        }

        private void LoadConfig(string configName)
        {

        }

        private void LoadDataTable(string dataTableName)
        {

        }

        private void LoadDictionary(string dictionaryName)
        {
            
        }

        private void LoadFont(string fontName)
        {
            
        }

        private void OnLoadConfigSuccess(object sender, GameEventArgs e)
        {

        }

        private void OnLoadConfigFailure(object sender, GameEventArgs e)
        {
            
        }

        private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        {

        }

        private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        {

        }

        private void OnLoadDictionarySuccess(object sender, GameEventArgs e)
        {

        }

        private void OnLoadDictionaryFailure(object sender, GameEventArgs e)
        {
            
        }
    }
}
