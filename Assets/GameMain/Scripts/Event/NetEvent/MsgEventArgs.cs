using GameFramework;
using GameFramework.Event;

public class MsgEventArgs<T> : GameEventArgs where T : BaseMsg
{
    //事件编号
    public static readonly int EventId = typeof(MsgEventArgs<T>).GetHashCode();

    //ID
    public override int Id
    {
        get { return EventId; }
    }

    private BaseMsg msg = new BaseMsg();

    //事件参数
    public T Msg
    {
        get { return (T)msg; }
        private set { msg = value; }
    }

    public static MsgEventArgs<T> Create(BaseMsg baseMsg)
    {
        MsgEventArgs<T> msgEventArgs = ReferencePool.Acquire<MsgEventArgs<T>>();
        msgEventArgs.msg = baseMsg;
        return msgEventArgs;
    }

    public override void Clear()
    {
        msg = null;
    }
}
