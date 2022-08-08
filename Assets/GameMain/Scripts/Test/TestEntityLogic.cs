using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityGameFramework.Runtime;

namespace NetworkBasedFPS
{
    class TestEntityLogic : EntityLogic
    {
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            Log.Debug("显示测试实体");
        }
    }
}
