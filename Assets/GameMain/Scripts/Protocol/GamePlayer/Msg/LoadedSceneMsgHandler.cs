namespace GamePlayer
{
	public class LoadedSceneMsgHandler : BaseHandler
	{
		public override void MsgHandle()
		{
			LoadedSceneMsg msg = message as LoadedSceneMsg;
		}
	}
}
