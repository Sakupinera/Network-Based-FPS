using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class SwapWeaponSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 切换武器成功事件编号。
        /// </summary>
        public static readonly int EventId = typeof(SwapWeaponSuccessEventArgs).GetHashCode();

        /// <summary>
        /// 初始化加载场景成功事件的新实例。
        /// </summary>
        public SwapWeaponSuccessEventArgs()
        {

        }

        /// <summary>
        /// 获取切换武器成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 获取切换武器时所按下的键位
        /// </summary>
        public KeyCode Key
        {
            get;
            private set;
        }

        public int PlayerID
        {
            get;
            private set;
        }

        public static SwapWeaponSuccessEventArgs Create(KeyCode key, int playerID)
        {
            SwapWeaponSuccessEventArgs args = ReferencePool.Acquire<SwapWeaponSuccessEventArgs>();
            args.Key = key;
            args.PlayerID = playerID;
            return args;
        }

        /// <summary>
        /// 清理切换武器成功事件。
        /// </summary>
        public override void Clear()
        {
            Key = 0;
        }
    }
}

