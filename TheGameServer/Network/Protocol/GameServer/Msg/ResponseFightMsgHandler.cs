namespace GameServer
{
	public class ResponseFightMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			ResponseFightMsg msg = message as ResponseFightMsg;
		}
	}
}
