namespace GamePlayer
{
    public class ChangeCampMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            ChangeCampMsg msg = message as ChangeCampMsg;
        }
    }
}
