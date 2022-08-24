namespace GameServer
{
	public class UpdateInfoHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			UpdateInfo msg = message as UpdateInfo;
		}
	}
}
