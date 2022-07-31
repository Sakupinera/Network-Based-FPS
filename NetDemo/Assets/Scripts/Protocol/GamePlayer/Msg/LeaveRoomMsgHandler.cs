namespace GamePlayer
{
	public class LeaveRoomMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			LeaveRoomMsg msg = message as LeaveRoomMsg;
		}
	}
}
