namespace GamePlayer
{
	public class CteateRoomMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			CteateRoomMsg msg = message as CteateRoomMsg;
		}
	}
}
