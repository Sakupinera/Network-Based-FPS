//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace NetworkBasedFPS
{
    /// <summary>
    /// 游戏模式。
    /// </summary>
    public enum GameMode : byte
    {
        /// <summary>
        /// 个人模式
        /// </summary>
        Solo,

        /// <summary>
        /// 团队模式
        /// </summary>
        Team,

        /// <summary>
        /// 生存模式。
        /// </summary>
        Survival,
    }
}
