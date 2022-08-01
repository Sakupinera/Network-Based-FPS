
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class SoloGame : GameBase
    {
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Solo;
            }
        }
    }
}
