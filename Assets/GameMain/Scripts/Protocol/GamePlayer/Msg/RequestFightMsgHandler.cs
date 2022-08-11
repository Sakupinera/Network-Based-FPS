namespace GamePlayer
{
	public class RequestFightMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			RequestFightMsg msg = message as RequestFightMsg;
		}
	}
}
