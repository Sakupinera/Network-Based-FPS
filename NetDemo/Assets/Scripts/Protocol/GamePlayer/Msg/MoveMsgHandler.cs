namespace GamePlayer
{
	public class MoveMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			MoveMsg msg = message as MoveMsg;
		}
	}
} 