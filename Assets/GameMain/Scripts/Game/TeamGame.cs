using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class TeamGame : GameBase
    {
        public override GameMode GameMode => GameMode.Team;

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
        }
    }
}
