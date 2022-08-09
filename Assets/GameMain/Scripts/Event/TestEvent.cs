using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetworkBasedFPS
{
    public class TestEvent : GameEventArgs
    {
        public static readonly int EventId = typeof(TestEvent).GetHashCode();
        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public string EventName
        {
            get;
            private set;
        }
        public static TestEvent Create(string name)
        {
            TestEvent testEvent = ReferencePool.Acquire<TestEvent>();
            testEvent.EventName = name;
            return testEvent;
        }
        public override void Clear()
        {
            EventName = null;
        }
    }
}
