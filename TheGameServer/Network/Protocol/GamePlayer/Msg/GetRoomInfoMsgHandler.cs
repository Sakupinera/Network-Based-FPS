using GameServer;

namespace GamePlayer
{
    public class GetRoomInfoMsgHandler : BaseHandler
    {
        public override void MsgHandle()
        {
            GetRoomInfoMsg msg = message as GetRoomInfoMsg;
            RoomMsgLogic.MsgGetRoomInfo(ServerSocket.GetInstance().GetClient(msg.id).player);
        }
    }
}
