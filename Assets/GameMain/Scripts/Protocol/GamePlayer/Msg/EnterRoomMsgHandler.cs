namespace GamePlayer
{
	public class EnterRoomMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			EnterRoomMsg msg = message as EnterRoomMsg;
		}
	}
}
