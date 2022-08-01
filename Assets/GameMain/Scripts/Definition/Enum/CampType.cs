//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace NetworkBasedFPS
{
    /// <summary>
    /// 阵营类型。
    /// </summary>
    public enum CampType : byte
    {
        Unknown = 0,

        /// <summary>
        /// 第一玩家阵营。
        /// </summary>
        BlueCamp,

        /// <summary>
        /// 第二玩家阵营。
        /// </summary>
        RedCamp,
    }
}
