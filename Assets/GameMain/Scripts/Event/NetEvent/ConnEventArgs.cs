using GameFramework;
using GameFramework.Event;

public class ConnEventArgs : GameEventArgs
{
    //事件编号
    public static readonly int EventId = typeof(ConnEventArgs).GetHashCode();

    //ID
    public override int Id
    {
        get { return EventId; }
    }

    public string connInfo
    {
        get;
        private set;
    }

    public static ConnEventArgs Create(string info)
    {
        ConnEventArgs connEventArgs = ReferencePool.Acquire<ConnEventArgs>();
        connEventArgs.connInfo = info;
        return connEventArgs;
    }

    public override void Clear()
    {
        connInfo = null;
    }
}
