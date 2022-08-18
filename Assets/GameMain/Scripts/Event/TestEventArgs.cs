using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(TestEventArgs).GetHashCode();
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
    public static TestEventArgs Create(string name)
    {
        TestEventArgs testEvent = ReferencePool.Acquire<TestEventArgs>();
        testEvent.EventName = name;
        return testEvent;
    }
    public override void Clear()
    {
        EventName = null;
    }
}
