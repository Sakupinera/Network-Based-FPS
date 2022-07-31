namespace GameServer
{
	public class GetListMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			GetListMsg msg = message as GetListMsg;
		}
	}
}
