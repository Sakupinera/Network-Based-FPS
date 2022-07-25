using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace NetworkBasedFPS {
    public class ProcedureBattle : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Debug("进入游戏流程，可以在这里处理游戏逻辑，这条日志不会打印，因为没有切换到Battle流程");
        }
    } 
}
