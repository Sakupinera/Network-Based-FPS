using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class KillEvent : GameEventArgs
    {
        public static readonly int EventId = typeof(KillEvent).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public static KillEvent Create()
        {
            KillEvent testEvent = ReferencePool.Acquire<KillEvent>();
            return testEvent;
        }
        public override void Clear()
        {
        }
    }
}
