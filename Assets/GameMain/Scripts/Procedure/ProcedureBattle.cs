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

            //GameEntry.Entity.ShowEntity<TestEntityLogic>(1, "Assets/GameMain/Entities/TestEntity.prefab", "EntityGroup");

            

            Log.Debug("开始战斗！");
        }
    } 
}
