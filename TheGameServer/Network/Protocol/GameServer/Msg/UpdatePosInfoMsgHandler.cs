namespace GameServer
{
	public class UpdatePosInfoMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			UpdatePosInfoMsg msg = message as UpdatePosInfoMsg;
		}
	}
}
