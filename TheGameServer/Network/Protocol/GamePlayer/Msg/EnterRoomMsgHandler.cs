using GameServer;

namespace GamePlayer
{
    public class EnterRoomMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            EnterRoomMsg msg = message as EnterRoomMsg;
            RoomMsgLogic.MsgEnterRoom(ServerSocket.GetInstance().GetClient(msg.id).player, msg.index);
        }
    }
}
