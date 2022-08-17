using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NetworkBasedFPS   
{
    internal class SingleGame : GameBase
    {
        public override GameMode GameMode => GameMode.Single;

        public override void Initialize()
        {

            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 11001)
            {
                Name = "Sakupinera",
                Position = new Vector3(15f, 1, 35f)
            });

            // 设置鼠标灵敏度
            GameEntry.Setting.SetInt("MouseSensitivity", 100);

            GameOver = false;
        }
    }
}
