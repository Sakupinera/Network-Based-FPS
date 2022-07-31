namespace GameServer
{
	public class UpdateStatusInfoMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			UpdateStatusInfoMsg msg = message as UpdateStatusInfoMsg;
		}
	}
}
