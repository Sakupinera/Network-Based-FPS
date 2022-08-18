using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlayerEventArgs : GameEventArgs
{
    public static readonly int EventId = typeof(AddPlayerEventArgs).GetHashCode();
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
    public static AddPlayerEventArgs Create(string name)
    {
        AddPlayerEventArgs testEvent = ReferencePool.Acquire<AddPlayerEventArgs>();
        testEvent.EventName = name;
        return testEvent;
    }
    public override void Clear()
    {
        EventName = null;
    }
}

