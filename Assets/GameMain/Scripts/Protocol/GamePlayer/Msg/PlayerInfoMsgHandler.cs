namespace GamePlayer
{
	public class PlayerInfoMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			PlayerInfoMsg msg = message as PlayerInfoMsg;
		}
	}
}
