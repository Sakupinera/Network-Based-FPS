namespace GameServer
{
	public class RoomListMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			RoomListMsg msg = message as RoomListMsg;
		}
	}
}
