using UnityEngine;

namespace NetworkBasedFPS
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static NetComponent Net
        {
            get;
            private set;
        }
        /// <summary>
        /// 自定义组件
        /// </summary>
        private static void InitCustomComponents()
        {
            Net = UnityGameFramework.Runtime.GameEntry.GetComponent<NetComponent>();
        }
    }
}
