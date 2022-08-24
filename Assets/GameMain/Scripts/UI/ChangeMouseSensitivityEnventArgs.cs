using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkBasedFPS
{
    public class ChangeMouseSensitivityEnventArgs : GameEventArgs
    {

        public static readonly int EventId = typeof(ChangeMouseSensitivityEnventArgs).GetHashCode();


        public ChangeMouseSensitivityEnventArgs()
        {
            MouseSensitivity = 100f;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取鼠标灵敏度大小。
        /// </summary>
        public float MouseSensitivity
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载场景失败事件。
        /// </summary>
        /// <param name="e">内部事件。</param>
        /// <returns>创建的加载场景失败事件。</returns>
        public static ChangeMouseSensitivityEnventArgs Create(float mouseSensitivity)
        {
            ChangeMouseSensitivityEnventArgs changeMouseSensitivityEnventArgs = ReferencePool.Acquire<ChangeMouseSensitivityEnventArgs>();
            changeMouseSensitivityEnventArgs.MouseSensitivity = mouseSensitivity;
            return changeMouseSensitivityEnventArgs;
        }

        public override void Clear()
        {

        }
    }
}
